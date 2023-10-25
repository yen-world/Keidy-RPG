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
    // Start is called before the first frame update
    void Start()
    {

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
        canMove = true;

    }
}
