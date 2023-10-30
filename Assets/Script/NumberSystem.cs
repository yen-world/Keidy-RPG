using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberSystem : MonoBehaviour
{
    // 사운드 출력을 위한 변수
    AudioManager theAudio;
    // 방향키 입력 사운드
    public string key_sound;
    // 결정키 입력 사운드
    public string enter_sound;
    // 오답, 취소 사운드
    public string cancel_sound;
    // 정답 사운드
    public string correct_sound;

    // 자물쇠의 자릿수
    int count;
    // 현재 선택된 자릿수
    int selectedTextBox;
    // 플레이어가 선택한 값
    int result;
    // 정답
    int correctNumber;
    // 입력된 숫자를 하나씩 더해서 숫자형으로 만들어줄 변수
    string tempNumber;

    // 화면에 정렬하기 위한 변수
    public GameObject superObject;
    public GameObject[] panel;
    public TMP_Text[] Number_Text;

    public Animator anim;

    // Coroutine에서 WaitUntil의 대기 조건으로 사용할 변수
    public bool activated;
    // 자물쇠 이벤트 발생 시 키 입력 방지 변수
    bool keyInput;
    // 플레이어의 입력 값과 정답과 일치 유무 확인 변수
    bool correctFlag;



    // Start is called before the first frame update
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    // 비밀번호 맞추는 이벤트를 실행하는 함수
    public void ShowNumber(int _correctNumber)
    {
        correctNumber = _correctNumber;
        // 비밀번호 이벤트가 실행중이면 true
        activated = true;
        // 비밀번호의 정답을 맞추면 true, 아니면 false, 초기값으로 false
        correctFlag = false;

        // 매개변수로 받아온 correctNumber가 몇자리 수인지 확인하기 위해 string으로 변한
        string temp = correctNumber.ToString();

        // 비밀번호 자릿수만큼 패널을 활성화하고 텍스트를 초기화
        for (int i = 0; i < temp.Length; i++)
        {
            count = i;
            panel[i].SetActive(true);
            Number_Text[i].text = "0";
        }

        // 비밀번호의 자릿수만큼 패널들을 오른쪽으로 이동
        superObject.transform.position = new Vector3(superObject.transform.position.x + 30 * count, superObject.transform.position.y, superObject.transform.position.z);

        // 현재 선택한 자릿수와 고른 정답을 초기화하고, 패널의 색상도 초기화,
        selectedTextBox = 0;
        result = 0;
        SetColor();
        anim.SetBool("Appear", true);

        // 이제부터 비밀번호 이벤트를 위한 키 입력을 활성화하고 Update에서 키 입력을 받음
        keyInput = true;
    }

    // 정답 유무를 bool 값으로 반환
    public bool GetResult()
    {
        return correctFlag;
    }

    // 위아래 키를 누르면 숫자를 세팅
    public void SetNumber(string _arrow)
    {
        // TMP_Text의 text는 string형이기 때문에 int형으로 형변환을 해준다.
        int temp = int.Parse(Number_Text[selectedTextBox].text);

        if (_arrow == "DOWN")
        {
            if (temp == 0)
                temp = 9;
            else
                temp--;
        }
        else if (_arrow == "UP")
        {
            if (temp == 9)
                temp = 0;
            else
                temp++;
        }
        Number_Text[selectedTextBox].text = temp.ToString();
    }

    // 현재 활성화된 숫자만 Alpha값을 255로 조절하고 나머지는 0.3f의 값으로 조절
    public void SetColor()
    {
        Color color = Number_Text[0].color;
        color.a = 0.3f;
        for (int i = 0; i <= count; i++)
        {
            Number_Text[i].color = color;
        }
        color.a = 1f;
        Number_Text[selectedTextBox].color = color;
    }

    void Update()
    {
        // 비밀번호 이벤트가 실행되면 키 입력을 받아오기
        if (keyInput)
        {
            // 아래키 or 위키를 누르면 비밀번호 숫자 세팅
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                theAudio.Play(key_sound);
                SetNumber("DOWN");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                theAudio.Play(key_sound);
                SetNumber("UP");
            }
            // 왼쪽키 or 오른쪽키를 누르면 비밀번호 자릿수 변경
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                theAudio.Play(key_sound);
                if (selectedTextBox < count)
                    selectedTextBox++;
                else
                    selectedTextBox = 0;
                SetColor();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {

                theAudio.Play(key_sound);
                if (selectedTextBox > 0)
                    selectedTextBox--;
                else
                    selectedTextBox = count;
                SetColor();
            }
            // Z키를 누르면 비밀번호 정답 제출
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                theAudio.Play(enter_sound);
                keyInput = false;
                StartCoroutine(OXCoroutine());

            }
            // X키를 누르면 비밀번호 이벤트 취소
            else if (Input.GetKeyDown(KeyCode.X))
            {
                theAudio.Play(cancel_sound);
                keyInput = false;
                StartCoroutine(ExitCoroutine());
            }
        }
    }

    // 정답 여부 확인 Coroutine
    IEnumerator OXCoroutine()
    {
        Color color = Number_Text[0].color;
        color.a = 1f;

        // 숫자들의 Alpha 값을 255로 전부 초기화하고 비밀번호 값 받아오고 1초 대기
        for (int i = count; i >= 0; i--)
        {
            Number_Text[i].color = color;
            tempNumber += Number_Text[i].text;
        }
        yield return new WaitForSeconds(1f);

        // 문자열로 받아온 tempNumber를 int형으로 강제형변환
        result = int.Parse(tempNumber);

        // 정답과 일치하면 correctFlag를 true로 변경
        if (result == correctNumber)
        {
            theAudio.Play(correct_sound);
            correctFlag = true;
        }
        // 정답이 아니라면 correctFlag를 false로 변경
        else
        {
            theAudio.Play(cancel_sound);
            correctFlag = false;
        }

        StartCoroutine(ExitCoroutine());
    }

    // 비밀번호 이벤트 취소 coroutine
    IEnumerator ExitCoroutine()
    {
        // 각종 변수 초기화 및 애니메이션 실행
        result = 0;
        tempNumber = "";
        selectedTextBox = 0;
        anim.SetBool("Appear", false);
        yield return new WaitForSeconds(0.1f);

        // Panel들을 SetActive(false)로 숨김처리하고, Text도 0으로 초기화
        for (int i = 0; i <= count; i++)
        {
            panel[i].SetActive(false);
            Number_Text[i].text = "0";
        }
        // 옮겨뒀던 SuperObject를 원래 자리로 되돌림
        superObject.transform.position = new Vector3(superObject.transform.position.x - 30 * count, superObject.transform.position.y, superObject.transform.position.z);

        // 비밀번호 이벤트 종료를 알림
        activated = false;
    }
}
