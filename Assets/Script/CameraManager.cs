using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 카메라가 따라다닐 오브젝트
    public GameObject target;
    public float moveSpeed;
    // 따라다닐 대상의 위치값(카메라가 이동해야 하는 값)
    Vector3 targetPosition;

    // 싱글톤 디자인 패턴을 구현하기 위한 static instance
    static public CameraManager instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target.gameObject != null)
        {
            // Z 값이 this인 이유는, 2D 템플릿에서는 카메라와 오브젝트의 Z 값에 차이가 있기 때문. 카메라에 붙여야 하기 때문에 this를 사용함
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
