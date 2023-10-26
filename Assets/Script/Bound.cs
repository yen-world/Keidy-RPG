using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    // 씬 이동 시 새로운 씬의 BoxCollider2D를 저장할 변수
    BoxCollider2D bound;

    // CameraManager의 SetBound를 사용하기 위한 변수
    CameraManager theCamera;


    // Start is called before the first frame update
    void Start()
    {
        bound = GetComponent<BoxCollider2D>();
        theCamera = FindObjectOfType<CameraManager>();
        theCamera.SetBound(bound);
    }


}
