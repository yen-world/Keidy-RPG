using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    static public ChoiceManager instance;

    // 사운드 재생
    AudioManager theAudio;

    // Choice 클래스에서 필드들을 받아올 변수
    string question;
    List<string> answerList;

    // 선택지 UI를 SetActive 시킬 목적으로 만든 변수
    public GameObject go;

    // 질문에 들어갈 Text, 선택에 들어갈 Text 배열, answer_Panel을 배열에 넣어 Alpah 값으로 강조하기 위한 GameObject 배열 
    public TMP_Text question_Text;
    public TMP_Text[] answer_Text;
    public GameObject[] answer_Panel;

    public Animator anim;

    // 키 입력 및 선택지 선택 시 출력할 Sound 이름
    public string keySound;
    public string enterSound;

    // 선택지가 등장했을때 다른 이벤트를 실행하지 않기 위한 변수
    public bool choiceIng;
    // 선택지 창이 열려있는지 여부에 따른 키 입력 처리 방지 변수
    bool keyInput;

    // 배열의 크기
    int count;
    // 현재 가리키고 있는 선택지의 번호
    int result;

    WaitForSeconds waitTime = new WaitForSeconds(0.01f);

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
        theAudio = FindObjectOfType<AudioManager>();
        answerList = new List<string>();

        for (int i = 0; i < answer_Text.Length; i++)
        {
            // 선택지 Text와 Panel을 초기화 및 비활성화
            answer_Text[i].text = "";
            answer_Panel[i].SetActive(false);
        }
        // 질문 Text를 초기화
        question_Text.text = "";
    }

    // 선택지 이벤트를 실행하는 함수
    public void ShowChoice(Choice _choice)
    {
        // 선택지 UI를 Active로 바꿔주고, 대화 상태 유무를 정하는 choiceIng를 true로, result는 0으로 초기화, question은 매개변수로 받아온다
        go.SetActive(true);
        choiceIng = true;
        result = 0;
        question = _choice.question;

        // 매개변수로 받아온 answer을 List에 추가하고 Panel도 Active로 바꿔줌
        for (int i = 0; i < _choice.answers.Length; i++)
        {
            answerList.Add(_choice.answers[i]);
            answer_Panel[i].SetActive(true);
            // 반복문을 다 돌고 나면 count == 배열의 크기가 됨
            count = i;
        }

        // 선택지가 출력되는 듯한 Animation 실행
        anim.SetBool("Appear", true);

        // 제일 첫번째 선택지가 선택된 듯한 연출을 하기 위해 실행
        Selection();

        // Coroutine에서 이벤트 처리
        StartCoroutine(ChoiceCoroutine());
    }

    // 선택지 이벤트 종료시 Object의 속성 및 변수 초기화
    public void ExitChoice()
    {
        question_Text.text = "";
        for (int i = 0; i <= count; i++)
        {
            answer_Text[i].text = "";
            answer_Panel[i].SetActive(false);
        }
        choiceIng = false;
        answerList.Clear();
        anim.SetBool("Appear", false);
        go.SetActive(false);
    }

    // 몇 번째 선택지를 선택했는지 결과값 반환
    public int GetResult()
    {
        return result;
    }

    // 활성화된 선택지를 강조하기 위한 함수
    public void Selection()
    {
        Color color = answer_Panel[0].GetComponent<Image>().color;
        color.a = 0.75f;

        // 모든 선택지 Panel을 동일하게 일치시키고
        for (int i = 0; i <= count; i++)
        {
            answer_Panel[i].GetComponent<Image>().color = color;
        }
        // 현재 활성화된 Panel의 Alpha 값만 조절
        color.a = 1f;
        answer_Panel[result].GetComponent<Image>().color = color;
    }

    IEnumerator ChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        // 선택지의 갯수가 1~4개이기 때문에 조건문을 달아서 몇 번째의 answer을 출력할지 정함
        StartCoroutine(TypingQuestion());
        StartCoroutine(TypingAnswer_0());
        if (count >= 1)
            StartCoroutine(TypingAnswer_1());
        if (count >= 2)
            StartCoroutine(TypingAnswer_2());
        if (count >= 3)
            StartCoroutine(TypingAnswer_3());

        yield return new WaitForSeconds(0.5f);

        // 선택지가 전부 출력이 되면 그때 키 입력으로 선택지를 고를 수 있게 해줌
        keyInput = true;
    }

    IEnumerator TypingQuestion()
    {
        for (int i = 0; i < question.Length; i++)
        {
            question_Text.text += question[i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer_0()
    {
        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < answerList[0].Length; i++)
        {
            answer_Text[0].text += answerList[0][i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer_1()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < answerList[1].Length; i++)
        {
            answer_Text[1].text += answerList[1][i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer_2()
    {
        yield return new WaitForSeconds(0.6f);

        for (int i = 0; i < answerList[2].Length; i++)
        {
            answer_Text[2].text += answerList[2][i];
            yield return waitTime;
        }
    }

    IEnumerator TypingAnswer_3()
    {
        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < answerList[3].Length; i++)
        {
            answer_Text[3].text += answerList[3][i];
            yield return waitTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 선택지가 모두 출력이 됐다면 키 입력 감지
        if (keyInput)
        {
            // 위쪽 방향키를 누르면
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                theAudio.Play(keySound);
                // 제일 위에 있는 선택지를 활성화 시킨 상태에서 위쪽 방향키를 누르면 맨 아래 선택지로 이동하고
                // 2~4번째 선택지를 가리키고 있다면 바로 위쪽 선택지를 활성화 시킬 수 있게 함
                if (result > 0)
                {
                    result--;
                }
                else
                {
                    result = count;
                }
                // 활성화된 선택지 강조 함수 실행
                Selection();
            }
            // 아래쪽 방향키를 누르면
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                theAudio.Play(keySound);
                // 배열의 크기가 4이고, 1~3번째 선택지를 가리키고 있다면 아래 선택지로 이동
                // 마지막 선택지를 가리키고 있다면 맨 위 선택지로 이동
                if (result < count)
                {
                    result++;
                }
                else
                {
                    result = 0;
                }
                // 활성화된 선택지 강조 함수 실행
                Selection();
            }
            // Z키를 눌러서 선택을 완료하면
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                // 선택 완료 오디오 실행하고 키 입력을 더 이상 받지 않게 하고, ExitChoice() 실행
                theAudio.Play(enterSound);
                keyInput = false;
                ExitChoice();
            }
        }
    }
}
