using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    static public Menu instance;

    // Menu UI를 담을 오브젝트
    public GameObject go;
    public AudioManager theAudio;
    public OrderManager theOrder;

    // Menu UI를 켤 때 재생할 사운드
    public string call_sound;
    // Menu UI를 끌 때 재생할 사운드
    public string cancel_sound;

    // Menu UI의 활성화 체크
    bool activated;

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

    // 게임 종료
    public void Exit()
    {
        Application.Quit();
    }

    // 계속
    public void Continue()
    {
        activated = false;
        theOrder.Move();
        go.SetActive(false);
        theAudio.Play(cancel_sound);
    }

    // Update is called once per frame
    void Update()
    {
        // Esc 키를 누르면 Menu 창 활성화
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activated = !activated;

            if (activated)
            {
                theOrder.NotMove();
                go.SetActive(true);
                theAudio.Play(call_sound);
            }
            else
            {
                theOrder.Move();
                go.SetActive(false);
                theAudio.Play(cancel_sound);
            }
        }

    }
}
