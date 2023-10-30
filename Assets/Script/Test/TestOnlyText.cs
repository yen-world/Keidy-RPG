using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOnlyText : MonoBehaviour
{
    OrderManager theOrder;
    DialogueManager theDM;
    public bool flag;
    public string[] texts;

    // Start is called before the first frame update
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theDM = FindObjectOfType<DialogueManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!flag)
        {
            StartCoroutine(ShowText());
        }
    }

    IEnumerator ShowText()
    {
        flag = true;
        theOrder.NotMove();
        theDM.ShowText(texts);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Move();
    }
}
