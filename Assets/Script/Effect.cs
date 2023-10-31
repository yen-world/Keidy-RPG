using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // 이펙트가 사라질 시간
    public float deleteTime;

    // Update is called once per frame
    void Update()
    {
        // 매 프레임마다 시간을 줄이면서 해당 시간이 지나면 이펙트는 사라지게함
        deleteTime -= Time.deltaTime;
        if (deleteTime <= 0)
            Destroy(this.gameObject);
    }
}
