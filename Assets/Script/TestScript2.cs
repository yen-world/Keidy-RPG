using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript2 : MonoBehaviour
{
    BGMManager BGM;

    // Start is called before the first frame update
    void Start()
    {
        BGM = FindObjectOfType<BGMManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(abc());
    }

    IEnumerator abc()
    {
        BGM.FadeOutMusic();

        yield return new WaitForSeconds(3f);

        BGM.FadeOutMusic();

    }
}
