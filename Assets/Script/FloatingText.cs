using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    // 화면에서 올라가는 연출을 줄 스피드
    public float moveSpeed;
    // 텍스트가 화면에 떠있을 시간
    public float destroyTime;
    public TMP_Text text;

    // 텍스트가 위치해야 하는 포지션 값을 지닌 벡터
    Vector3 vector;


    // Update is called once per frame
    void Update()
    {
        // 매 프레임마다 벡터의 위치를 설정하고 text의 position에 대입
        vector.Set(text.transform.position.x, text.transform.position.y + (moveSpeed * Time.deltaTime), text.transform.position.z);
        text.transform.position = vector;

        // destroyTime을 매 프레임마다 감소시키고 0보다 작아지면 텍스트 파괴
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
