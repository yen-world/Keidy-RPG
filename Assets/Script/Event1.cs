using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1 : MonoBehaviour
{
    // 대화를 두 번 진행하기에 두 개 생성
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    // 다른 스크립트의 메서드를 사용하기 위한 변수
    DialogueManager theDM;
    OrderManager theOrder;
    PlayerManager thePlayer;

    // OntriggerStay가 한 번만 실행되도록 제어하는 변수
    bool flag;

    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // OntriggerStay 함수가 처음 호출되고, Z키를 누르고, Player가 위쪽을 바라보고 있을때 이벤트를 실행한다.
        if (!flag && Input.GetKey(KeyCode.Z) && thePlayer.animator.GetFloat("DirY") == 1f)
        {
            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        // Character 오브젝트들을 받아오고 이동 불가 상태로 지정
        theOrder.PreLoadCharacter();
        theOrder.NotMove();

        // 1번째 대화를 실행하고 대화가 종료될 때까지 WaitUntil로 대기
        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);

        // 이동 이벤트를 실행하고 이동 명령이 끝난 후 0.5초까지 대기
        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "UP");
        yield return new WaitUntil(() => thePlayer.queue.Count == 0);
        yield return new WaitForSeconds(0.5f);

        // 2번째 대화를 실행하고 대화가 종료될 때까지 WaitUntil로 대기
        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        // Character들을 움직일 수 있게 제어
        theOrder.Move();
    }
}
