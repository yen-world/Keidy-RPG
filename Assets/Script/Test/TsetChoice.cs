using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TsetChoice : MonoBehaviour
{
    [SerializeField]
    public Choice choice;
    OrderManager theOrder;
    ChoiceManager theChoice;
    bool flag;
    // Start is called before the first frame update
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!flag)
        {
            StartCoroutine(ACoroutine());
            flag = true;
        }
    }

    IEnumerator ACoroutine()
    {
        flag = true;
        // 선택지가 출력되기 전에 캐릭터의 움직임을 제어
        theOrder.NotMove();
        // 선택지 출력
        theChoice.ShowChoice(choice);
        // 선택지가 출력이 되고 Player가 선택을 완료할때까지 대기
        yield return new WaitUntil(() => !theChoice.choiceIng);

        // 다시 캐릭터의 움직임을 활성화
        theOrder.Move();
        Debug.Log(theChoice.GetResult());
    }
}
