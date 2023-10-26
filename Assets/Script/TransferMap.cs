using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // transferPoint에 collider가 감지된 객체의 이름이 Player라면
        if (other.gameObject.name == "Player")
        {
            // transferMapName의 씬으로 이동
            SceneManager.LoadScene(transferMapName);
        }
    }
}
