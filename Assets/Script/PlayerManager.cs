using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{
    public float runSpeed;
    float applyRunSpeed;

    // 달리기 중일때 2타일씩 건너뛰는 것을 방지하기 위한 변수
    bool applyRunFlag = false;

    // Coroutine의 반복 실행을 제어하기 위한 변수
    bool canMove = true;

    // 이동하고자 하는 맵의 이름을 TransferMap에서 받아오기 위한 변수
    public string currentMapName;

    // 현재 캐릭터가 있는 씬의 위치 이름
    public string currentSceneName;

    // 싱글톤 디자인 패턴을 구현하기 위한 static instance
    static public PlayerManager instance;

    // 플레이어가 걸을 때 실행할 sound clip 이름
    public string walkSound_1;
    public string walkSound_2;
    public string walkSound_3;
    public string walkSound_4;

    SaveAndLoad theSaveLoad;
    AudioManager theAudio;

    // Player가 대화중일때 이동하지 못하게 하는 변수
    public bool notMove = false;

    // 공격중인지 아닌지 체크
    bool attacking = false;
    // 공격 대기 시간(공격 속도)
    public float attackDelay;
    // 공격 대기 시간 확인하는 변수
    float currentAttackDelay;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        // AudioManager가 붙은 AudioObject 불러오기
        theAudio = FindObjectOfType<AudioManager>();
        queue = new Queue<string>();
        theSaveLoad = FindObjectOfType<SaveAndLoad>();
    }

    void Update()
    {
        // 세이브
        if (Input.GetKeyDown(KeyCode.F5))
        {
            theSaveLoad.CallSave();
        }

        // 로드
        if (Input.GetKeyDown(KeyCode.F9))
        {
            theSaveLoad.CallLoad();
        }

        if (canMove && !notMove && !attacking)
        {
            // 방향키 입력 감지
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }

        if (!notMove && !attacking)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentAttackDelay = attackDelay;
                attacking = true;
                animator.SetBool("Attacking", true);
            }
        }

        if (attacking)
        {
            currentAttackDelay -= Time.deltaTime;
            if (currentAttackDelay <= 0)
            {
                animator.SetBool("Attacking", false);
                attacking = false;
            }
        }
    }

    // 캐릭터의 이동이 끝날때까지 대기하게 하기 위한 코루틴
    IEnumerator MoveCoroutine()
    {
        // 처음 키를 눌러서 Coroutine에 진입을 했고, 키를 계속 누르고 있다면 While문 안의 내용을 계속 반복 실행 
        while (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && !notMove && !attacking)
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

            bool checkCollisionFlag = base.CheckCollision();
            if (checkCollisionFlag)
                break;

            animator.SetBool("Walking", true);

            // 걸을때마다 랜덤하게 walkSound1~4의 sound 출력
            int temp = Random.Range(1, 4);
            switch (temp)
            {
                case 1:
                    theAudio.Play(walkSound_1);
                    break;
                case 2:
                    theAudio.Play(walkSound_2);
                    break;
                case 3:
                    theAudio.Play(walkSound_3);
                    break;
                case 4:
                    theAudio.Play(walkSound_4);
                    break;
            }

            // boxCollider를 미리 움직이고자 하는 방향으로 살짝 움직여줌
            boxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount, vector.y * 0.7f * speed * walkCount);

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

                // 절반 이상 걸어왔을 때 boxCollider의 offset을 원위치 시켜줌
                if (currentWalkCount == 12)
                {
                    boxCollider.offset = Vector2.zero;
                }

                currentWalkCount++;
                // 캐릭터가 자연스럽게 움직이기 위해 while문 안에 WaitForSeconds를 배치
                yield return new WaitForSeconds(0.01f);

                // 캐릭터가 걸을때마다 오디오가 재생되게 하는 코드

            }
            currentWalkCount = 0;
        }

        // canMove를 밑으로 빼는 이유는 키를 계속 누르고 있다면 canMove가 true가 되고, Update 함수에서 Coroutine를 계속 실행하기 때문에. Animator도 마찬가지.
        canMove = true;
        animator.SetBool("Walking", false);
    }
}
