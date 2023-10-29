using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    // 이벤트 도중에 키입력 방지
    PlayerManager thePlayer;

    // 한 맵에 생성할 Player, NPC의 정보들
    List<MovingObject> characters;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    // characters List에 Player 및 NPC를 받아오기
    public void PreLoadCharacter()
    {
        characters = ToList();
    }

    // Player 및 NPC 객체를 List에 담아서 반환해주는 함수
    public List<MovingObject> ToList()
    {
        List<MovingObject> tempList = new List<MovingObject>();

        // 현재 씬에 MovingObject를 가진 모든 객체를 반환하기에 MovingObject[]로 선언
        MovingObject[] temp = FindObjectsOfType<MovingObject>();

        for (int i = 0; i < temp.Length; i++)
        {
            tempList.Add(temp[i]);
        }
        return tempList;
    }

    public void NotMove()
    {
        thePlayer.notMove = true;
    }

    public void Move()
    {
        thePlayer.notMove = false;
    }

    // character 객체 충돌 감지 비활성화 함수
    public void SetThorought(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].boxCollider.enabled = false;
            }
        }
    }

    // character 객체 충돌 감지 활성화 함수
    public void SetUnThorought(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].boxCollider.enabled = true;
            }
        }
    }

    // character 객체 비활성화 함수
    public void SetTransparent(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].gameObject.SetActive(false);
            }
        }
    }

    // character 객체 활성화 함수
    public void SetUnTransparent(string _name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].gameObject.SetActive(true);
            }
        }
    }

    // 씬에 존재하는 Player나 NPC를 이벤트성으로 이동시키게 하는 함수
    public void Move(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].Move(_dir);
            }
        }
    }

    // 씬에 존재하는 Player나 NPC를 이벤트성으로 방향 전환을 시키는 함수
    public void Turn(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].animator.SetFloat("DirX", 0f);
                characters[i].animator.SetFloat("DirY", 0f);

                switch (_dir)
                {

                    case "UP":
                        characters[i].animator.SetFloat("DirY", 1f);
                        break;
                    case "DOWN":
                        characters[i].animator.SetFloat("DirY", -1f);
                        break;
                    case "LEFT":
                        characters[i].animator.SetFloat("DirX", -1f);
                        break;
                    case "RIGHT":
                        characters[i].animator.SetFloat("DirX", 1f);
                        break;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
