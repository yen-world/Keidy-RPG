using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    // NotMove를 위한 OrderManager, 사운드 출력을 위한 AudioManager, 플레이어 스탯 반영을 위한 PlayerStat,
    // 착용 확인 UI를 위한 UseOrCancel
    OrderManager theOrder;
    AudioManager theAudio;
    PlayerStat thePlayerStat;
    Inventory theInven;
    UseOrCancel theUse;

    // 각종 사운드 이름
    public string key_sound;
    public string enter_soound;
    public string open_sound;
    public string close_sound;
    public string takeoff_sound;
    public string equip_sound;

    // 장비 부위 이름을 상수로 만들기 위한 변수들
    const int WEAPON = 0, SHILED = 1, AMULT = 2, LEFT_RING = 3, RIGHT_RING = 4,
              HELMET = 5, ARMOR = 6, LEFT_GLOVE = 7, RIGHT_GLOVE = 8, BELT = 9,
              LEFT_BOOTS = 10, RIGHT_BOOTS = 11;

    // 캐릭터 능력치 이름을 상수로 만들기 위한 변수들
    const int ATK = 0, DEF = 1, STR = 2, DEX = 3, INT = 4, LUK = 5,
              HPR = 6, MPR = 7, CRI = 8, AVD = 9, HIT = 10;

    // 장비 아이템 등으로 추가된 능력치
    public int added_atk, added_def, added_hpr, added_mpr;

    // 장비창 전체 UI를 담을 오브젝트
    public GameObject go;

    // 게임 씬에 보여지는 플레이어가 착용하고 있는 무기 오브젝트
    public GameObject equipWeapon;

    // 장비창의 능력치 텍스트를 세팅할 텍스트
    public TMP_Text[] text;
    // 장비창의 장비 슬롯에 들어갈 아이템 이미지들
    public Image[] img_slots;
    // 어떤 장비칸이 선택됐는지 알려주는 강조 오브젝트
    public GameObject go_selected_Slot_UI;
    // 장비아이템 착용해제, 취소를 알려줄 UI 오브젝트
    public GameObject go_use;

    // 착용중인 장비 아이템 리스트
    public Item[] equipItemList;

    // 장비칸 슬롯 중 어느 슬롯을 선택했는지 int형으로 담을 변수
    int selectedSlot;

    // 장비창이 열려있는지 체크
    public bool activated = false;
    // 장비창이 열려있는 동안 다른 상호작용을 하지 못하게
    bool inputKey = true;

    void Start()
    {
        theInven = FindObjectOfType<Inventory>();
        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theUse = FindObjectOfType<UseOrCancel>();
    }

    // 장비창에 플레이어의 스탯을 보여주는 함수
    public void ShowText()
    {
        // 추가된 공격력이 0이라면 기본 공격력을 그대로 출력하고,
        // 추가된 공격력이 있다면 추가된 공격력이 몇인지 출력함
        if (added_atk == 0)
            text[ATK].text = thePlayerStat.atk.ToString();
        else
            text[ATK].text = thePlayerStat.atk.ToString() + "(+" + added_atk + ")";

        if (added_def == 0)
            text[DEF].text = thePlayerStat.def.ToString();
        else
            text[DEF].text = thePlayerStat.def.ToString() + "(+" + added_def + ")";

        if (added_hpr == 0)
            text[HPR].text = thePlayerStat.recovery_Hp.ToString();
        else
            text[HPR].text = thePlayerStat.recovery_Hp.ToString() + "(+" + added_hpr + ")";

        if (added_mpr == 0)
            text[MPR].text = thePlayerStat.recovery_Mp.ToString();
        else
            text[MPR].text = thePlayerStat.recovery_Mp.ToString() + "(+" + added_mpr + ")";

    }

    // 장비 아이템의 부위 확인
    public void EquipItem(Item _item)
    {
        string temp = _item.itemID.ToString();
        // 아이템 ID를 앞의 3자리 가져와서 어떤 종류의 아이템인지 체크
        temp = temp.Substring(0, 3);
        switch (temp)
        {
            case "200":
                EquipItemCheck(WEAPON, _item);
                // 무기를 장착했다면 무기를 활성화시켜주고, 무기의 이미지를 바꿔준다.
                equipWeapon.SetActive(true);
                equipWeapon.GetComponent<SpriteRenderer>().sprite = _item.itemIcon;
                break;
            case "201":
                EquipItemCheck(SHILED, _item);
                break;
            case "202":
                EquipItemCheck(AMULT, _item);
                break;
            case "203":
                EquipItemCheck(LEFT_RING, _item);
                break;
            case "204":
                EquipItemCheck(RIGHT_RING, _item);
                break;
            case "205":
                EquipItemCheck(HELMET, _item);
                break;
            case "206":
                EquipItemCheck(ARMOR, _item);
                break;
            case "207":
                EquipItemCheck(LEFT_GLOVE, _item);
                break;
            case "208":
                EquipItemCheck(RIGHT_GLOVE, _item);
                break;
            case "209":
                EquipItemCheck(BELT, _item);
                break;
            case "210":
                EquipItemCheck(LEFT_BOOTS, _item);
                break;
            case "211":
                EquipItemCheck(RIGHT_BOOTS, _item);
                break;
        }
    }

    // 장비 아이템 착용
    public void EquipItemCheck(int _count, Item _item)
    {
        // 착용하고자 하는 아이템(equipItemList[_count])가 비어있다면 아이템을 해당 칸에 넣어줌
        if (equipItemList[_count].itemID == 0)
        {
            equipItemList[_count] = _item;
        }
        // 이미 착용하고 있다면 기존에 착용하던 장비를 인벤토리로 보내고 새로운 장비를 착용함
        else
        {
            theInven.EquipToInventory(equipItemList[_count]);
            equipItemList[_count] = _item;
        }

        // 장비 아이템 착용시 플레이어 스탯에 반영
        EquipEffect(_item);
        theAudio.Play(equip_sound);
        // 변경된 플레이어 스탯을 보여줌
        ShowText();
    }

    // 선택중인 장비창을 강조
    public void SelectedSlot()
    {
        go_selected_Slot_UI.transform.position = img_slots[selectedSlot].transform.position;
    }

    // 장비창에 착용중인 아이템들을 띄움
    public void ShowEquip()
    {
        Color color = img_slots[0].color;
        color.a = 1f;

        // 아이템 슬롯의 개수만큼(12개)
        for (int i = 0; i < img_slots.Length; i++)
        {
            // 만약 착용중인 아이템의 ID가 0이 아니라면(버그 방지)
            if (equipItemList[i].itemID != 0)
            {
                // 아이템의 spirte와 색상을 변경
                img_slots[i].sprite = equipItemList[i].itemIcon;
                img_slots[i].color = color;
            }
        }
    }

    // 장비창의 착용 아이템들을 비움
    public void ClearEquip()
    {
        Color color = img_slots[0].color;
        color.a = 0f;

        for (int i = 0; i < img_slots.Length; i++)
        {
            img_slots[i].sprite = null;
            img_slots[i].color = color;
        }
    }

    void Update()
    {
        if (inputKey)
        {
            // E키를 누르면 인벤토리 활성화
            if (Input.GetKeyDown(KeyCode.E))
            {
                activated = !activated;

                // 장비창을 켤 때
                if (activated)
                {
                    // 캐릭터의 움직임을 제한하고, 사운드를 출력하고, 장비창 활성화하고 첫번째 슬롯을 선택함
                    theOrder.NotMove();
                    theAudio.Play(open_sound);
                    go.SetActive(true);
                    selectedSlot = 0;

                    SelectedSlot();
                    ClearEquip();
                    ShowEquip();
                    ShowText();
                }
                // 장비창을 끌 때
                else
                {
                    // 캐릭터의 움직임을 허용하고, 사운드를 출력하고, 장비창을 비활성화함
                    theOrder.Move();
                    theAudio.Play(close_sound);
                    go.SetActive(false);
                    ClearEquip();
                }
            }
            // 장비창을 켰다면
            if (activated)
            {
                // 아래키 or 오른쪽키를 누르면 선택중인 슬롯의 칸을 아래로 옮김
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (selectedSlot < img_slots.Length - 1)
                        selectedSlot++;
                    else
                        selectedSlot = 0;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                // 아래키 or 오른쪽키를 누르면 선택중인 슬롯의 칸을 위로 옮김
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = img_slots.Length - 1;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectedSlot > 0)
                        selectedSlot--;
                    else
                        selectedSlot = img_slots.Length - 1;
                    theAudio.Play(key_sound);
                    SelectedSlot();
                }
                // Z키를 누르면 해당 장비칸에 아이템이 있다면 착용해제 확인 UI가 뜸
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (equipItemList[selectedSlot].itemID != 0)
                    {
                        theAudio.Play(enter_soound);
                        inputKey = false;
                        StartCoroutine(UseCoroutine("해제", "취소"));
                    }
                }
            }
        }
    }

    // 장비 아이템 장착시 장비 아이템의 효과를 플레이어 스탯에 반영
    void EquipEffect(Item _item)
    {
        thePlayerStat.atk += _item.atk;
        thePlayerStat.def += _item.def;
        thePlayerStat.recovery_Hp += _item.recover_hp;
        thePlayerStat.recovery_Mp += _item.recover_mp;

        added_atk += _item.atk;
        added_def += _item.def;
        added_hpr += _item.recover_hp;
        added_mpr += _item.recover_mp;
    }

    // 장비 아이템 해제시 장비 아이템의 효과를 플레이어 스탯에서 삭제
    void TakeOffEffect(Item _item)
    {
        thePlayerStat.atk -= _item.atk;
        thePlayerStat.def -= _item.def;
        thePlayerStat.recovery_Hp -= _item.recover_hp;
        thePlayerStat.recovery_Mp -= _item.recover_mp;

        added_atk -= _item.atk;
        added_def -= _item.def;
        added_hpr -= _item.recover_hp;
        added_mpr -= _item.recover_mp;
    }

    // 착용해제 UI 출력
    IEnumerator UseCoroutine(string _up, string _down)
    {
        // 착용해제 확인 UI를 출력
        go_use.SetActive(true);
        theUse.ShowTowChoice(_up, _down);
        yield return new WaitUntil(() => !theUse.activated);

        // 착용해제를 선택했다면 
        if (theUse.GetResult())
        {
            // 현재 착용중인 부위의 아이템을 인벤토리로 보내고, 비어있는 슬롯을 초기화해줌
            theInven.EquipToInventory(equipItemList[selectedSlot]);
            TakeOffEffect(equipItemList[selectedSlot]);
            // 만약 무기를 해제했다면 무기를 비활성화 시켜준다
            if (selectedSlot == WEAPON)
                equipWeapon.SetActive(false);
            ShowText();
            equipItemList[selectedSlot] = new Item(0, "", "", Item.ItemType.Equip);
            theAudio.Play(takeoff_sound);
            ClearEquip();
            ShowEquip();
        }
        inputKey = true;
        go_use.SetActive(false);
    }
}
