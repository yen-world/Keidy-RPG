using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    // 플레이어가 바라보고 있는 방향을 알기 위함
    PlayerManager thePlayer;
    Vector2 vector;

    // 회전(각)을 담당하는 Vector4
    Quaternion rotation;


    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // 손전등이 Player를 따라다니게 위치를 조절
        this.transform.position = thePlayer.transform.position;

        // Player의 Animation Parameter 값을 받아와서 어느 방향을 보고 있는지 vector에 저장
        vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));

        // 오른쪽을 바라보고 있을 때
        if (vector.x == 1f)
        {
            rotation = Quaternion.Euler(0, 0, 90);
            this.transform.rotation = rotation;
        }
        // 왼쪽을 바라보고 있을 때
        else if (vector.x == -1f)
        {
            rotation = Quaternion.Euler(0, 0, -90);
            this.transform.rotation = rotation;
        }
        // 위쪽을 바라보고 있을 때
        else if (vector.y == 1f)
        {
            rotation = Quaternion.Euler(0, 0, 180);
            this.transform.rotation = rotation;
        }
        // 아래쪽을 바라보고 있을 때
        else if (vector.y == -1f)
        {
            rotation = Quaternion.Euler(0, 0, 0);
            this.transform.rotation = rotation;
        }
    }
}
