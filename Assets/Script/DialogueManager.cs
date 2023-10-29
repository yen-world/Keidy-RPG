using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // 싱글톤 패턴을 위한 instance
    static public DialogueManager instance;

    // 대사를 띄울 TMP, 캐릭터 스프라이트를 띄울 SpriteRenderer와 대사창을 띄울 SpriteRenderer
    public TMP_Text text;
    public SpriteRenderer rendererSprite;
    public SpriteRenderer rendererDialogueWindow;

    // 상황마다 가변적인 대화의 길이로 인해서 크기 변경이 자유로운 List형으로 변수 선언
    // 이 List는 Dialouge에 있는 필드들을 추가함
    List<string> listSentences;
    List<Sprite> listSprites;
    List<Sprite> listDialogueWindows;

    // 대화 진행 상황 카운트
    int count;

    // 캐릭터 Sprite 및 대화창 Sprite의 애니메이션을 위한 Animator
    public Animator animSprite;
    public Animator animDialogueWindow;

    // 대화중일때는 캐릭터의 이동을 하지 못하게 만드는 bool 변수
    public bool talking = false;

    // 텍스트 타이핑 Sound, 대사 넘기는 Sound의 이름
    public string typeSound;
    public string enterSound;

    AudioManager theAudio;

    // 연속된 키 입력으로 인해 Animation 누락을 방지하는 bool 변수
    bool keyActivated = false;

    #region  Singleton
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
    #endregion Singleton

    // Start is called before the first frame update
    void Start()
    {
        // 변수 초기화 및 스크립트 가져오기
        count = 0;
        text.text = ""; // text는 숨겨놓지 않지만, ""으로 초기화해줌으로써 화면에서 사라져보이는 효과
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();
        theAudio = FindObjectOfType<AudioManager>();
    }

    // 대화를 시작하는 함수
    public void ShowDialogue(Dialogue dialogue)
    {
        // 대화를 시작하면 talking을 true로 만들어서 캐릭터의 이동을 제어
        talking = true;

        // 대사의 길이만큼 각 List에 Dialouge 필드들을 추가
        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
            listSprites.Add(dialogue.sprites[i]);
            listDialogueWindows.Add(dialogue.dialogueWindows[i]);
        }

        // 캐릭터 Sprite, 대사창 Sprite를 활성화(Visible)
        animSprite.SetBool("Appear", true);
        animDialogueWindow.SetBool("Appear", true);

        StartCoroutine(StartDialogueCoroutine());

    }

    // 대화 종료시 모든 변수 초기화 및 Sprite Animation 전환
    public void ExitDialogue()
    {
        animSprite.SetBool("Appear", false);
        animDialogueWindow.SetBool("Appear", false);
        text.text = "";
        count = 0;

        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();

        // 대화가 종료되면 Player가 다시 움직일 수 있게함
        talking = false;
    }

    IEnumerator StartDialogueCoroutine()
    {
        // 두 번째 이상의 대화라면
        if (count > 0)
        {   // 현재 대화창과 다음 대화창이 서로 다르다면 Animation을 주어서 바꿔줌
            if (listDialogueWindows[count] != listDialogueWindows[count - 1])
            {
                // 캐릭터 Sprite의 Alpha 값을 0으로 조절해서 숨김
                animSprite.SetBool("Change", true);
                // 대화창 Sprite를 밑으로 내림
                animDialogueWindow.SetBool("Appear", false);

                yield return new WaitForSeconds(0.2f);

                // 캐릭터 Sprite와 대화창 Sprite를 변경
                rendererDialogueWindow.sprite = listDialogueWindows[count];
                rendererSprite.sprite = listSprites[count];

                // 밑으로 내려뒀던 대화창을 다시 올림
                animDialogueWindow.SetBool("Appear", true);
                // Alpha 값을 0으로 조절했던 캐릭터 Sprite를 255로 조절
                animSprite.SetBool("Change", false);

            }
            // 현재 대화창이 다음 대화창과 같다면? 대화창은 애니메이션 적용을 안해도됨
            else
            {
                // 캐릭터 Sprite가 다르다면
                if (listSprites[count] != listSprites[count - 1])
                {
                    // 캐릭터 Sprite의 Alpha 값을 0으로 조절
                    animSprite.SetBool("Change", true);

                    yield return new WaitForSeconds(0.1f);

                    // 캐릭터 Sprite를 변경
                    rendererSprite.sprite = listSprites[count];

                    // Alpha 값을 0으로 주었던 Sprite를 255로 조절
                    animSprite.SetBool("Change", false);
                }
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        // 첫번째 대화라면 스프라이트들을 띄워줌
        else
        {
            rendererDialogueWindow.sprite = listDialogueWindows[count];
            rendererSprite.sprite = listSprites[count];
        }

        // 대사를 빠르게 넘겨서 Sprite가 사라지는 것을 방지하기 위해 true로 설정
        keyActivated = true;

        // 받아온 대사를 하나씩 끊어서 0.01f초 단위로 TMP에 추가함
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i];
            // 텍스트가 일정 갯수만큼 출력될 경우 타이핑 Sound 출력
            if (i % 7 == 1)
            {
                theAudio.Play(typeSound);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 대화중이고 Sprite가 정상 출력 됐다면 키 입력 감지
        if (talking && keyActivated)
        {
            // Z키를 누르면 대사 넘김
            if (Input.GetKeyDown(KeyCode.Z))
            {
                keyActivated = false;
                // 다음 대사로 count 증가
                count++;
                // 이전 대사와 겹치지 않기 위해 기존 텍스트 삭제
                text.text = "";
                // 대사 넘김 사운드 출력
                theAudio.Play(enterSound);

                // 마지막 대사라면 Coroutine을 종료하고 대화 종료
                if (count == listSentences.Count)
                {
                    StopAllCoroutines();
                    ExitDialogue();
                }
                // 아직 대사가 남았다면 이전 대사를 끝내고 다음 대사로 넘김
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartDialogueCoroutine());
                }
            }
        }

    }
}
