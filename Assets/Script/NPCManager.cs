using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC의 움직임을 담당하는 클래스
[System.Serializable]
public class NPCMove
{
    [Tooltip("NPCMove를 체크하면 NPC가 움직임")]
    public bool NPCmove;

    // NPC가 움직일 방향 설정
    public string[] direction;

    // NPC가 움직일 방향으로 얼마나 빠른 속도로 움직일 것인지 설정하는 변수
    [Range(1, 5)]
    [Tooltip("1 = 천천히, 2 = 조금 천천히, 3 = 보통, 4 = 빠르게, 5 = 연속적으로")]
    public int frequency;
}
public class NPCManager : MovingObject
{
    [SerializeField]
    public NPCMove npc;
    // Start is called before the first frame update
    void Start()
    {
        queue = new Queue<string>();
    }

    public void SetMove()
    {
        StartCoroutine(MoveCoroutine());
    }

    public void SetNotMove()
    {
        StopAllCoroutines();
    }

    // MoveCoroutine은 NPC를 직접적으로 이동시키는 함수가 아니라, 무한히 NPC의 이동을 "실행"할 수 있게 해주는 함수 
    IEnumerator MoveCoroutine()
    {
        // NPC의 이동 방향이 있다면 MoveCoroutine을 무한히 반복
        if (npc.direction.Length != 0)
        {
            for (int i = 0; i < npc.direction.Length; i++)
            {
                // NPC의 이동 텀에 따라 대기 시간을 따로 줌
                switch (npc.frequency)
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
                // queue.Count가 2보다 작을때 여기서 무한히 대기, true가 되면 빠져나옴
                yield return new WaitUntil(() => queue.Count < 2);

                // 실질적인 이동 구간으로 이동에 대한 코드는 부모 클래스인 MovingObject에 있음
                base.Move(npc.direction[i], npc.frequency);

                // NPC의 이동 싸이클이 끝났다면 다시 처음부터 시작하게 함
                if (i == npc.direction.Length - 1)
                {
                    i = -1;
                }
            }
        }
    }
}
