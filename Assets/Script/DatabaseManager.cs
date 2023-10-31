using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;

    // 아이템 사용시 플레이어의 스탯에 접근하기 위한 변수
    PlayerStat thePlayerStat;

    // 아이템 사용시 플레이어의 스탯이 증가되는것을 보여주기 위한 플로팅 텍스트 및 Canvas
    public GameObject prefabs_Floating_Text;
    public GameObject parent;

    // 각종 변수의 이름과 실수 값을 저장해놓을 배열. ex) 골드, 500
    public string[] var_name;
    public float[] var;

    // 각종 이벤트들의 이름과 실행 여부를 저장해놓을 배열. ex) 학교 입장 컷신, true
    public string[] switch_name;
    public bool[] switches;

    public List<Item> itemList = new List<Item>();

    // 아이템 사용시 캐릭터가 받는 효과를 플로팅 텍스트로 출력
    void FloatText(int _number, string color)
    {
        Vector3 vector = thePlayerStat.transform.position;
        vector.y += 60;

        GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = _number.ToString();
        if (color == "GREEN")
        {
            clone.GetComponent<FloatingText>().text.color = Color.green;
        }
        else if (color == "BLUE")
        {
            clone.GetComponent<FloatingText>().text.color = Color.blue;
        }
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);
    }

    // 아이템 ID에 따른 효과를 부여함
    public void UseItem(int _itemId)
    {
        switch (_itemId)
        {
            // 빨간 포션은 체력을 50 채워줌. 현재 체력에서 50을 채운 값이 최대 체력보다 낮다면 50을 채워주고 아니라면 최대 체력까지만 회복
            case 10001:
                if (thePlayerStat.hp >= thePlayerStat.currentHP + 50)
                    thePlayerStat.currentHP += 50;
                else
                    thePlayerStat.currentHP = thePlayerStat.hp;
                FloatText(50, "GREEN");
                break;

            // 파란 포션은 마나를 15 채워줌. 현재 마나에서 15을 채운 값이 최대 마나보다 낮다면 15을 채워주고 아니라면 최대 마나까지만 회복
            case 10002:
                if (thePlayerStat.mp >= thePlayerStat.currentMP + 15)
                    thePlayerStat.currentMP += 15;
                else
                    thePlayerStat.currentMP = thePlayerStat.mp;
                FloatText(50, "BLUE");
                break;
        }
    }

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
        thePlayerStat = FindObjectOfType<PlayerStat>();
        itemList.Add(new Item(10001, "빨간 포션", "체력을 50 채워주는 기적의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10002, "파란 포션", "마나를 15 채워주는 기적의 물약", Item.ItemType.Use));
        itemList.Add(new Item(10003, "농축 빨간 포션", "체력을 350 채워주는 기적의 농축 물약", Item.ItemType.Use));
        itemList.Add(new Item(10004, "농축 파란 포션", "마나를 80 채워주는 기적의 농축 물약", Item.ItemType.Use));
        itemList.Add(new Item(11001, "랜덤 상자", "랜덤으로 포션이 나온다. 낮은 확률로 꽝", Item.ItemType.Use));
        itemList.Add(new Item(20001, "짧은 검", "기본적인 용사의 검", Item.ItemType.Equip));
        itemList.Add(new Item(20301, "사파이어 반지", "1분에 마나 1을 회복시켜주는 마법 반지", Item.ItemType.Equip));
        itemList.Add(new Item(30001, "고대 유물의 조각 1", "반으로 쪼개진 고대 유물의 파편", Item.ItemType.Quest));
        itemList.Add(new Item(30002, "고대 유물의 조각 2", "반으로 쪼개진 고대 유물의 파편", Item.ItemType.Quest));
        itemList.Add(new Item(30003, "고대 유물", "고대 유적에 잠들어있던 고대의 유물", Item.ItemType.Quest));
    }


}
