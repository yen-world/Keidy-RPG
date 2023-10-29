using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    // 대화 스크립트
    [TextArea(1, 2)]
    public string[] sentences;
    // 캐릭터 스프라이트
    public Sprite[] sprites;
    // 대화창 스프라이트
    public Sprite[] dialogueWindows;

}
