using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNumber : MonoBehaviour
{
    NumberSystem theNumber;
    OrderManager theOrder;
    bool flag;
    public int correctNumber;
    // Start is called before the first frame update
    void Start()
    {
        theNumber = FindObjectOfType<NumberSystem>();
        theOrder = FindObjectOfType<OrderManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && !flag)
        {
            StartCoroutine(NumberCoroutine());
        }
    }

    IEnumerator NumberCoroutine()
    {
        flag = true;
        theOrder.NotMove();
        theNumber.ShowNumber(correctNumber);
        yield return new WaitUntil(() => !theNumber.activated);
        theOrder.Move();
    }
}
