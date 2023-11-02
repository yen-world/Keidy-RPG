using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public string characterName;
    // 캐릭터의 이동속도
    public float speed;

    // 캐릭터를 픽셀 단위로 이동하기 위한 변수
    public int walkCount;
    protected int currentWalkCount;

    public Animator animator;

    // 방향키 입력 값을 받아오기 위한 Vector
    protected Vector3 vector;

    // 캐릭터가 통과 불가능한 영역을 구분하기 위한 레이어 마스크
    public LayerMask layerMask;

    public BoxCollider2D boxCollider;

    public Queue<string> queue;

    bool notCoroutine = false;

    // NPC의 이동을 시키는 실질적인 함수, 방향과 속도를 받아옴
    public void Move(string _dir, int _frequency = 5)
    {
        queue.Enqueue(_dir);
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }
    }

    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        while (queue.Count != 0)
        {
            // NPC의 이동 텀에 따라 대기 시간을 따로 줌
            switch (_frequency)
            {
                case 1:
                    yield return new WaitForSeconds(4f);
                    break;
                case 2:
                    yield return new WaitForSeconds(3f);
                    break;
                case 3:
                    yield return new WaitForSeconds(2f);
                    break;
                case 4:
                    yield return new WaitForSeconds(1f);
                    break;
                case 5:
                    break;
            }

            string direction = queue.Dequeue();

            vector.Set(0, 0, vector.z);
            switch (direction)
            {
                case "UP":
                    vector.y = 1f;
                    break;
                case "DOWN":
                    vector.y = -1f;
                    break;
                case "RIGHT":
                    vector.x = 1f;
                    break;
                case "LEFT":
                    vector.x = -1f;
                    break;
            }

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            while (true)
            {
                bool checkCollisionFlag = CheckCollision();
                // Player가 NPC의 진행방향을 가로막고 있다면 대기
                if (checkCollisionFlag)
                {
                    animator.SetBool("Walking", false);
                    yield return new WaitForSeconds(1f);
                }
                // Player가 NPC의 방향에서 비켜났으면 탈출하고 MoveCoroutine을 마저 실행
                else
                {
                    break;
                }
            }

            animator.SetBool("Walking", true);

            // boxCollider를 미리 움직이고자 하는 방향으로 살짝 움직여줌
            boxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount, vector.y * 0.7f * speed * walkCount);

            // 48픽셀만큼 움직이지 않았을 경우 이동을 계속함
            while (currentWalkCount < walkCount)
            {
                transform.Translate(vector.x * speed, vector.y * speed, 0);
                currentWalkCount++;
                // 절반 이상 걸어왔을 때 boxCollider의 offset을 원위치 시켜줌
                if (currentWalkCount == walkCount * 0.5f + 2)
                {
                    boxCollider.offset = Vector2.zero;
                }
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;

            // NPC가 연속적으로 움직이고 있다면 Walking 애니메이션을 끄면 안되므로 조건문을 달아 필터링
            if (_frequency != 5)
                animator.SetBool("Walking", false);
        }
        animator.SetBool("Walking", false);
        notCoroutine = false;
    }

    protected bool CheckCollision()
    {
        // start는 현재 캐릭터의 위치, end는 캐릭터가 이동하고자 하는 위치
        // Ray를 start에서 end로 쏴서 방해물이 있는지 없는지 확인하기 위한 변수들
        RaycastHit2D hit;

        // 캐릭터의 현재위치와 캐릭터가 나아갈 위치를 대입
        Vector2 start = new Vector2(transform.position.x + vector.x * speed * walkCount,
                                    transform.position.y + vector.y * speed * walkCount);
        Vector2 end = start + new Vector2(vector.x * speed, vector.y * speed);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true;

        if (hit.transform != null)
            return true;
        return false;
    }
}
