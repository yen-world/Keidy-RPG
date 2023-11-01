using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    // 씬 이동 시 새로운 씬의 BoxCollider2D를 저장할 변수
    BoxCollider2D bound;

    // CameraManager의 SetBound를 사용하기 위한 변수
    CameraManager theCamera;

    // 맵마다 bound가 있기 때문에 bound를 같이 불러와야 로드했을때 카메라가 다른 bound에 갇히는 현상이 없어짐
    public string boundName;

    // Start is called before the first frame update
    void Start()
    {
        bound = GetComponent<BoxCollider2D>();
        theCamera = FindObjectOfType<CameraManager>();
    }

    public void SetBound()
    {
        if (theCamera != null)
        {
            theCamera.SetBound(bound);
        }
    }
}
