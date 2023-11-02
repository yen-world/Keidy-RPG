using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject[] gos;
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

    // 타이틀로 가는 버튼 클릭시 실행
    public void GoToTitle()
    {
        // 기존에 객체들의 정보를 가지고 타이틀로 넘어가지 않게 모든 오브젝트를 삭제
        // 삭제 후 재생성되면 값이 초기화가 되기 때문
        for (int i = 0; i < gos.Length; i++)
            Destroy(gos[i]);

        go.SetActive(false);
        activated = false;
        SceneManager.LoadScene("Title");

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
