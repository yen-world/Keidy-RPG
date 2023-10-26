using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // 방향키 입력 값을 받아오기 위한 Vector
    Vector3 vector;

    // 캐릭터의 이동속도
    public float speed;
    public float runSpeed;
    float applyRunSpeed;

    // 달리기 중일때 2타일씩 건너뛰는 것을 방지하기 위한 변수
    bool applyRunFlag = false;

    // 캐릭터를 픽셀 단위로 이동하기 위한 변수
    public int walkCount;
    int currentWalkCount;

    // Coroutine의 반복 실행을 제어하기 위한 변수
    bool canMove = true;

    Animator animator;
    BoxCollider2D boxCollider;

    // 캐릭터가 통과 불가능한 영역을 구분하기 위한 레이어 마스크
    public LayerMask layerMask;

    // start는 현재 캐릭터의 위치, end는 캐릭터가 이동하고자 하는 위치
    // Ray를 start에서 end로 쏴서 방해물이 있는지 없는지 확인하기 위한 변수들
    RaycastHit2D hit;
    Vector2 start;
    Vector2 end;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            // 방향키 입력 감지
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());

            }
        }

    }

    // 캐릭터의 이동이 끝날때까지 대기하게 하기 위한 코루틴
    IEnumerator MoveCoroutine()
    {
        // 처음 키를 눌러서 Coroutine에 진입을 했고, 키를 계속 누르고 있다면 While문 안의 내용을 계속 반복 실행 
        while (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            // 왼쪽 쉬프트를 누르면 이동 속도 증가 및 플래그 설정
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }


            // 방향키가 눌리는대로 vector에 받아오기
            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            // 좌우 또는 상하 방향키를 눌렀을 경우 y 또는 x 값이 동시에 설정되지 않게 하기 위해서 0으로 설정
            if (vector.x != 0)
                vector.y = 0;
            if (vector.y != 0)
                vector.x = 0;

            // Animator의 Parameter를 vector의 값(Input.GetAxisRaw)로 바꿔줌
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            // 캐릭터의 현재위치와 캐릭터가 나아갈 위치를 대입
            start = transform.position;
            end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true;

            if (hit.transform != null)
            {
                break;
            }

            animator.SetBool("Walking", true);

            // 48픽셀만큼 움직이지 않았을 경우 이동을 계속함
            while (currentWalkCount < walkCount)
            {
                // 좌 or 우 방향키가 눌렸을 경우
                if (vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                // 상 or 하 방향키가 눌렸을 경우
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }

                // 달리는 중이라면 currentWalkCount 증가를 1번 추가 진행
                if (applyRunFlag)
                {
                    currentWalkCount++;
                }
                currentWalkCount++;
                // 캐릭터가 자연스럽게 움직이기 위해 while문 안에 WaitForSeconds를 배치
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }

        // canMove를 밑으로 빼는 이유는 키를 계속 누르고 있다면 canMove가 true가 되고, Update 함수에서 Coroutine를 계속 실행하기 때문에. Animator도 마찬가지.
        canMove = true;
        animator.SetBool("Walking", false);
    }
}
