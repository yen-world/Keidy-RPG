using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    // 씬 이동(맵 이동)시 플레이어가 등장할 씬
    public string startPoint;

    // Player와 Camera를 startPoint에 위치하게 하기 위해서 변수로 받아옴
    MovingObject thePlayer;
    CameraManager theCamera;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<MovingObject>();
        theCamera = FindObjectOfType<CameraManager>();

        // 이동하고자 하는 씬의 이름(transferMapName)과 startPoint가 같은 씬으로 이동
        if (startPoint == thePlayer.currentMapName)
        {
            thePlayer.transform.position = this.transform.position;
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, theCamera.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
