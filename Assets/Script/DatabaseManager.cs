using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;

    // 각종 변수의 이름과 실수 값을 저장해놓을 배열. ex) 골드, 500
    public string[] var_name;
    public float[] var;

    // 각종 이벤트들의 이름과 실행 여부를 저장해놓을 배열. ex) 학교 입장 컷신, true
    public string[] switch_name;
    public bool[] switches;

    void Awake()
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
    // Start is called before the first frame update
    void Start()
    {

    }


}
