using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferScene : MonoBehaviour
{
    // 이동하고자 하는 씬의 이름
    public string transferMapName;
    MovingObject thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<MovingObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // transferPoint에 collider가 감지된 객체의 이름이 Player라면
        if (other.gameObject.name == "Player")
        {
            thePlayer.currentMapName = transferMapName;
            // transferMapName의 씬으로 이동
            SceneManager.LoadScene(transferMapName);
        }
    }
}
