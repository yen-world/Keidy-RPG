using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MovingObject
{
    // 공격 딜레이
    public float attackDelay;
    // 이동 전 대기시간
    public float inter_MoveWaitTime;
    // 대기 시간이 얼마나 지났는지 확인하는 변수
    float current_interMWT;
    // 슬라임의 공격 사운드
    public string atkSound;

    // 플레이어의 좌표값
    Vector2 playerPos;

    // 랜덤으로 움직이기 위한 변수
    int random_int;
    // 슬라임이 바라보고 있는 방향
    string direction;

    // Start is called before the first frame update
    void Start()
    {
        // MovingObject에 Object의 입력값들을 Queue로 받아서 처리하기 때문에 슬라임의 이동경로를 저장할 queue 생성
        queue = new Queue<string>();
        // 행동 대기시간을 초기화
        current_interMWT = inter_MoveWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        // 행동 대기시간이 지났다면 다음 행동(이동 or 공격)을 실행
        current_interMWT -= Time.deltaTime;
        if (current_interMWT <= 0)
        {
            current_interMWT = inter_MoveWaitTime;

            // 슬라임 근처에 플레이어가 있는지 체크
            if (NearPlayer())
            {
                Flip();
                return;
            }

            // 랜덤하게 이동방향 설정
            RandomDirection();

            // 이동하고자 하는 방향에 방해물이 있으면 취소
            if (base.CheckCollision())
                return;

            // 이동하고자 하는 방향에 방해물이 있다면 이동
            base.Move(direction);
        }
    }

    // 슬라임 공격 시 좌우 반전
    void Flip()
    {
        // 슬라임의 scale을 받아와서 Vector3에 저장
        Vector3 flip = transform.localScale;
        // 플레이어가 슬라임의 오른쪽에 있다면 오른쪽을 바라보게 scale.x 반전
        if (playerPos.x > this.transform.position.x)
            flip.x = -1f;
        // 플레이어가 왼쪽에 있다면 scale.x를 1으로 초기화
        else
            flip.x = 1f;

        this.transform.localScale = flip;
        animator.SetTrigger("Attack");

        StartCoroutine(WaitCoroutine());
    }

    // 공격 딜레이를 주는 Coroutine
    IEnumerator WaitCoroutine()
    {
        // 공격 딜레이만큼 대기하고 공격 사운드 출력
        yield return new WaitForSeconds(attackDelay);
        AudioManager.instance.Play(atkSound);

        // 공격 모션이 이미 실행중이고, 여전히 플레이어가 옆에 있다면 데미지를 입힘(없으면 공격 모션만 나오고 데미지는 입히지 않음)
        if (NearPlayer())
            PlayerStat.instance.Hit(GetComponent<EnemyStat>().atk);
    }

    // 슬라임이 랜덤한 방향으로 움직이게 하는 함수
    void RandomDirection()
    {
        vector.Set(0, 0, vector.z);
        // 0~3까지
        random_int = Random.Range(0, 4);
        switch (random_int)
        {
            case 0:
                vector.y = 1f;
                direction = "UP";
                break;
            case 1:
                vector.y = -1f;
                direction = "DOWN";
                break;
            case 2:
                vector.x = 1f;
                direction = "RIGHT";
                break;
            case 3:
                vector.x = -1f;
                direction = "LEFT";
                break;
        }
    }

    // 슬라임 근처에 플레이어가 있는지 체크
    bool NearPlayer()
    {
        playerPos = PlayerManager.instance.transform.position;

        // 가로로 같은 선상에 있다면 true 반환
        if (Mathf.Abs((Mathf.Abs(playerPos.x) - Mathf.Abs(this.transform.position.x))) <= speed * walkCount * 1.01f)
        {
            if (Mathf.Abs((Mathf.Abs(playerPos.y) - Mathf.Abs(this.transform.position.y))) <= speed * walkCount * 0.5f)
            {
                return true;
            }
        }

        // 세로로 같은 선상에 있다면 true 반환
        if (Mathf.Abs((Mathf.Abs(playerPos.y) - Mathf.Abs(this.transform.position.y))) <= speed * walkCount * 1.01f)
        {
            if (Mathf.Abs((Mathf.Abs(playerPos.x) - Mathf.Abs(this.transform.position.x))) <= speed * walkCount * 0.5f)
            {
                return true;
            }
        }

        // 플레이어가 없다면 false 반환
        return false;
    }
}
