using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    // 아이템 고유의 ID
    public int itemID;
    // 아이템의 이름
    public string itemName;
    // 아이템의 설명
    public string itemDescription;
    // 아이템의 갯수
    public int itemCount;
    // 아이템의 아이콘
    public Sprite itemIcon;
    // 아이템의 타입(Use, Equip, Quest, ETC)
    public ItemType itemType;

    // enum은 열거형으로 이 외에 값을 가지지 못한다
    public enum ItemType
    {
        Use,
        Equip,
        Quest,
        ETC
    }

    public int atk;
    public int def;
    public int recover_hp;
    public int recover_mp;


    // Item 클래스의 생성자. Item 객체를 생성할 때 Parameter를 넘겨주어서 초기화한다.
    // Sprite 이름과 itemId 이름이 같아서 Sprit의 이름은 따로 받지 않았지만, 만약 Sprite 이미지의 이름이 다르다면 String으로 불러와준다.
    // 아이템의 생성자로 Id, Name, Description, Type, 공격력, 방어력, 체력회복, 마나회복 등을 인수로 받아와서 생성된 Item 객체를 초기화한다.
    public Item(int _itemId, string _itemName, string _itemDes, ItemType _itemType,
                int _atk = 0, int _def = 0, int _recover_hp = 0, int _recover_mp = 0, int _itemCount = 1)
    {
        itemID = _itemId;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        itemCount = _itemCount;
        // 
        itemIcon = Resources.Load("ItemIcon/" + _itemId.ToString(), typeof(Sprite)) as Sprite;

        atk = _atk;
        def = _def;
        recover_hp = _recover_hp;
        recover_mp = _recover_mp;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
