using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    FadeManager theFade;
    AudioManager theAudio;
    PlayerManager thePlayer;
    GameManager theGM;

    // 타이틀 메뉴 클릭시 출력 사운드
    public string click_sound;

    // Start is called before the first frame update
    void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theGM = FindObjectOfType<GameManager>();
    }

    public void StartGame()
    {
        StartCoroutine(GameStartCoroutine());
    }

    // 처음 시작 버튼 클릭시 실행
    IEnumerator GameStartCoroutine()
    {
        // FadeOut연출과 사운드 재생
        theFade.FadeOut();
        theAudio.Play(click_sound);
        yield return new WaitForSeconds(2f);

        // Title 씬에서 캐릭터의 Alpha 값을 0으로 숨겨뒀던걸 다시 255로 초기화
        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;

        // 캐릭터의 처음 시작 위치 및 시작 씬
        thePlayer.currentMapName = "forest";
        thePlayer.currentSceneName = "start";

        theGM.LoadStart();

        SceneManager.LoadScene("start");
    }

    // 게임 종료 버튼 클릭시 실행
    public void ExitGame()
    {
        theAudio.Play(click_sound);
        Application.Quit();
    }
}
