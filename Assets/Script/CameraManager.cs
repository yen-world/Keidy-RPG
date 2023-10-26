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

    // 카메라의 영역을 지정한 콜라이더
    public BoxCollider2D bound;

    // 카메라 영역 콜라이더의 최대, 최소값의 x,y,z 벡터 값
    Vector3 minBound;
    Vector3 maxBound;

    // 카메라의 피봇이 중앙에 있기 때문에 카메라의 피봇이 Collider에 부딪히더라도 영역을 절반 벗어나기 때문에 그 값을 더해줄 변수
    float halfWidth;
    float halfHeight;

    // Camera의 width, height를 구하기 위한 변수
    Camera theCamera;

    private void Awake()
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

    void Start()
    {
        theCamera = GetComponent<Camera>();

        // bound의 collider 최소, 최대 영역 저장
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;

        // Camera의 절반 크기 저장
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.gameObject != null)
        {
            // Z 값이 this인 이유는, 2D 템플릿에서는 카메라와 오브젝트의 Z 값에 차이가 있기 때문. 카메라에 붙여야 하기 때문에 this를 사용함
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }
    }

    // 새로운 씬 or 맵으로 이동할 때 새로운 영역의 BoxCollider를 현재 bound로 새롭게 설정하는 함수
    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}
