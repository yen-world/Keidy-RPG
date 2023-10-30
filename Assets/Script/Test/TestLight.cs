using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLight : MonoBehaviour
{
    public GameObject go;
    bool flag;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!flag)
        {
            go.SetActive(true);
            flag = true;
        }
    }
}
