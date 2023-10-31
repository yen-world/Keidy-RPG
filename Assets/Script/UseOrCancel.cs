using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UseOrCancel : MonoBehaviour
{
    // 아이템 사용, 취소, 키입력 사운드
    AudioManager theAudio;
    // 키입력 사운드
    public string key_sound;
    // 사용 사운드
    public string enter_sound;
    // 취소 사운드
    public string cancel_sound;

    // 사용 버튼을 가리기위한 패널
    public GameObject up_Panel;
    // 취소 버튼을 가리기위한 패널
    public GameObject down_Panel;

    // 사용, 장착 등 텍스트를 출력하기 위한 텍스트
    public TMP_Text up_Text;
    // 취소, 벗기 등 텍스트를 출력하기 위한 텍스트
    public TMP_Text down_Text;

    // 확인 UI가 실행중인지 체크하는 변수
    public bool activated;
    // 동시에 키입력을 방지하기 위한 변수
    bool keyInput;
    // 아이템을 사용한것인지 아닌지 체크하는 변수
    bool result = true;


    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void Selected()
    {
        theAudio.Play(key_sound);
        // 아래키를 누른다면 false로 변환, 위키를 누른다면 true로 변환
        result = !result;

        // 사용탭이 활성화됐다면
        if (result)
        {
            up_Panel.gameObject.SetActive(false);
            down_Panel.gameObject.SetActive(true);
        }
        // 취소탭이 활성화됐다면
        else
        {
            up_Panel.gameObject.SetActive(true);
            down_Panel.gameObject.SetActive(false);
        }
    }

    // 아이템을 사용할 것인지 확인 UI를 띄워주는 함수
    public void ShowTowChoice(string _upText, string _downText)
    {
        // 현재 확인 UI가 활성화 됐음을 알리는 activated
        activated = true;
        // 사용 패널을 먼저 활성화
        result = true;
        // 장착인지, 사용인지 텍스트를 받아와서 출력
        up_Text.text = _upText;
        down_Text.text = _downText;

        // 제일 처음엔 사용 패널을 비활성화 시켜서 텍스트가 보이게 만들고
        // 취소 패널을 활성화 시켜서 텍스트를 가리게 만든다
        up_Panel.gameObject.SetActive(false);
        down_Panel.gameObject.SetActive(true);

        StartCoroutine(ShowTwoChoiceCoroutine());
    }

    // 아이템을 Z키로 사용을 하는데 확인 UI를 건너뛰는 것을 방지하기 위해서 0.01초의 대기시간을 가지고 keyInput을 true로 바꿔줌
    IEnumerator ShowTwoChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        keyInput = true;
    }

    // 결과 반환(사용이면 true, 취소면 false)
    public bool GetResult()
    {
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        // 확인 UI가 뜨고나서 키 입력이 가능해진 상태가 된다면
        if (keyInput)
        {
            // 위아래 방향키로 사용 or 취소를 선택함
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Selected();
            }
            // Z키를 누른다면 사용효과음을 출력하고, 이제 activted를 false로 바꿔서 확인 UI 작업이 끝났음을 알림
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                theAudio.Play(enter_sound);
                keyInput = false;
                activated = false;
            }
            // X키를 누른다면 취소효과음을 출력하고, 확인 UI 작업이 끝났음을 알리고, 사용 취소를 했다는 뜻인 result를 false로 바꿔줌
            else if (Input.GetKeyDown(KeyCode.X))
            {
                theAudio.Play(cancel_sound);
                keyInput = false;
                activated = false;
                result = false;
            }
        }
    }
}
