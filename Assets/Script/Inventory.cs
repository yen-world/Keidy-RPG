using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    static public Inventory instance;
    DatabaseManager theDatabase;
    // 캐릭터 이동 제어와 오디오 출력을 위한 변수
    OrderManager theOrder;
    AudioManager theAudio;
    UseOrCancel theUse;
    Equipment theEquip;

    // 인벤토리 내 키입력 사운드
    public string key_sound;
    // 아이템 사용 사운드
    public string enter_sound;
    // 아이템 취소 및 인벤토리 취소 사운드
    public string cancel_sound;
    // 인벤토리 Open 사운드
    public string open_sound;
    // 부정 사운드
    public string beep_sound;

    // 인벤토리 '슬롯' 배열
    InventorySlot[] slots;
    // 소지한 아이템 리스트
    List<Item> inventoryItemList;
    // 탭별 아이템 리스트
    List<Item> inventoryTabList;

    // 아이템 설명 텍스트
    public TMP_Text description_Text;
    // 탭 설명 텍스트
    public string[] tabDescription;

    // Slot 부모 객체(Grid Slot)의 Transform -> 부모 객체를 이용해서 Slot에 접근하기 위함
    public Transform tf;

    // 인벤토리 SetActive하기 위한 변수
    public GameObject go;
    // 탭 선택시, 강조되게 보이게 하는 Panel들
    public GameObject[] selectedTabImages;
    // 아이템 사용, 취소 오브젝트
    public GameObject go_UseOrCancel;
    // 아이템을 획득하면 화면에 출력될 텍스트 오브젝트
    public GameObject prefab_floating_Text;


    // 현재 선택된 아이템의 순서 -> 1이면 2번 아이템(파란 포션 등)
    int selectedItem;
    // 현재 선택된 탭의 순서 -> 1이면 2번 탭(장비)
    int selectedTab;

    // 인벤토리가 켜져있는지 체크
    bool activated;
    // 현재 탭을 선택하고 있는지 체크
    bool tabActivated;
    // 현재 아이템을 선택하고 있는지 체크
    bool itemActivated;
    // 아이템 사용시 키 입력 방지
    bool stopKeyInput;
    // 중복실행 제한 체크
    bool preventExec;

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
        theOrder = FindObjectOfType<OrderManager>();
        theAudio = FindObjectOfType<AudioManager>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        theUse = FindObjectOfType<UseOrCancel>();
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        // Grid Slot의 자식 객체인 Slot들을 싹 다 slots에 넣어줌
        slots = tf.GetComponentsInChildren<InventorySlot>();
        theEquip = FindObjectOfType<Equipment>();
    }

    public List<Item> SaveItem()
    {
        return inventoryItemList;
    }

    public void LoadItem(List<Item> _itmeList)
    {
        inventoryItemList = _itmeList;
    }

    public void EquipToInventory(Item _item)
    {
        inventoryItemList.Add(_item);
    }

    // 아이템 습득 및 추가 시 실행되는 함수
    public void GetAnItem(int _itemID, int _count = 1)
    {
        // 데이터베이스에 등록된 아이템의 갯수만큼 반복
        for (int i = 0; i < theDatabase.itemList.Count; i++)
        {
            // 습득한 아이템의 ID와 데이터베이스에 존재하는 아이템의 ID와 일치하는 경우
            if (_itemID == theDatabase.itemList[i].itemID)
            {
                // clone이란 변수에 floating_text 프리팹을 넣고, 플레이어의 위치에 복제
                // 복제됨과 동시에 floating_text 프리팹에는 스크립트가 달려있어서 update함수를 돌다가 파괴될것임
                var clone = Instantiate(prefab_floating_Text, PlayerManager.instance.transform.position, Quaternion.Euler(Vector3.zero));
                // clone의 text 컴포넌트를 가져와서 해당 아이템 이름에 접근해 새로 세팅
                clone.GetComponent<FloatingText>().text.text = theDatabase.itemList[i].itemName + "를 " + _count + "개 획득하였습니다.";
                // clone의 부모 위치를 변경(this는 Inventory, Inventory는 Canvas의 자식 관계로 있으니 Canvas의 자식으로 clone이 들어가게 됨)
                clone.transform.SetParent(this.transform);

                // 자신의 인벤토리 아이템 갯수만큼 반복
                for (int j = 0; j < inventoryItemList.Count; j++)
                {
                    // 만약 자신이 가지고 있는 아이템이고 소모품일 경우에만 _count만큼 갯수 증가하고 종료
                    if (_itemID == inventoryItemList[j].itemID && inventoryItemList[j].itemType == Item.ItemType.Use)
                    {
                        inventoryItemList[j].itemCount += _count;
                        return;
                    }
                }
                // 자신이 가진 아이템을 다 찾아봐도 없다면 인벤토리에 갯수만큼 추가 후 종료
                inventoryItemList.Add(theDatabase.itemList[i]);
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        Debug.LogError("데이터베이스에 해당 ID값을 가진 아이템이 존재하지 않습니다.");
    }

    // 아이템 창을 탭 선택하기 이전에 안보이게 비워두고 탭 선택을 함
    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    }

    // 슬롯 초기화 함수
    public void RemoveSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }

    // 탭 선택 함수
    public void SelectedTab()
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        // 모든 탭 Panel들의 Alpha 값을 0으로 바꿔줌
        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        // 설명 텍스트를 현재 선택된 탭의 설명으로 바꿔줌
        description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    }

    // 선택된 탭이 반짝거리는 효과를 주는 Coroutine
    IEnumerator SelectedTabEffectCoroutine()
    {
        while (tabActivated)
        {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            // 탭 Panel의 Alpha 값을 절반까지 올려줌
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            // 그리고 탭 Panel의 Alpha 값을 다시 0으로 낮춰줌
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    // 탭에 맞는 아이템들만 넣어주고 인벤토리 슬롯에 출력
    public void ShowItem()
    {
        // TabList를 초기화하지 않으면 타입이 다른 아이템끼리 묶어서 출력 될 가능성이 있다.
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;

        // 현재 탭이 1번이라면 case 1번으로 가서 itemType이 Equip인 아이템만 inventoryTabList에 추가한다
        switch (selectedTab)
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        }

        // 탭별 아이템의 갯수만큼 slots을 활성화시키고 slots에 아이템을 추가한다
        for (int i = 0; i < inventoryTabList.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].AddItem(inventoryTabList[i]);
        }

        // Item 목록을 보여주면 첫번째 아이템이 선택되게
        SelectedItem();
    }

    // 아이템 선택시 설명 텍스트를 바꾸고 Alpha 값을 조절
    public void SelectedItem()
    {
        StopAllCoroutines();
        // 예를 들어 소비탭에 아이템을 하나라도 가지고 있어야 설명 텍스트를 변경하고, Alpha 값을 변경해줌
        if (inventoryTabList.Count > 0)
        {
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i < inventoryTabList.Count; i++)
                slots[i].selected_Item.GetComponent<Image>().color = color;
            description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        // 하나도 없다면 없다는 텍스트 출력
        else
        {
            description_Text.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
        }
    }

    // 선택된 아이템이 반짝거리는 효과를 주는 Coroutine
    IEnumerator SelectedItemEffectCoroutine()
    {
        // Item 목록을 활성화 시켯다면
        while (itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            // 아이템 슬롯 Panel의 Alpha 값을 절반까지 올려줌
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            // 그리고 아이템 슬롯 Panel의 Alpha 값을 다시 0으로 낮춰줌
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }



    // Update is called once per frame
    void Update()
    {
        // 키 입력을 할 수 있을때 I키를 누르면 인벤토리 진입
        if (!stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                // 인벤토리가 닫혀있다면 true로 바꿔서 인벤토리를 열어주고
                // 인벤토리가 열려있다면 false로 바꿔서 인벤토리를 닫아준다
                activated = !activated;

                // 인벤토리를 킬 때
                if (activated)
                {
                    // 인벤토리 Open 사운드 출력, 플레이어 이동 제한, 인벤토리 Active, 현재 탭 0으로 초기화, 탭을 선택하고 있고 아이템은 선택할 수 없게
                    theAudio.Play(open_sound);
                    theOrder.NotMove();
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();
                }
                // 인벤토리를 끌때
                else
                {
                    // 인벤토리 Close 사운드 출력, 인벤토리 UnActive, 변수 초기화, 플레이어 이동 재개
                    theAudio.Play(cancel_sound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    theOrder.Move();
                }
            }

            // 인벤토리가 켜져있다면
            if (activated)
            {
                // 그리고 탭이 활성화되어있다면
                if (tabActivated)
                {
                    // 오른쪽 키를 누르면 seletedTab이 오른쪽으로 움직이게
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedTab < selectedTabImages.Length - 1)
                            selectedTab++;
                        else
                            selectedTab = 0;
                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    // 왼쪽 키를 누르면 seletedTab이 왼쪽으로 움직이게
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedTab > 0)
                            selectedTab--;
                        else
                            selectedTab = selectedTabImages.Length - 1;
                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    // Z키를 누르면 아이템 목록이 활성화되게함
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        // 현재 선택한 탭의 색상을 변경하고 오디오 실행
                        theAudio.Play(enter_sound);
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;

                        // 아이템 목록을 활성화 시킬거니까 itemActivated를 true, 탭 목록은 비활성화 시킬거니까 false, Z키가 중복실행 되지 않게 preventExec를 true로 설정
                        itemActivated = true;
                        tabActivated = false;
                        preventExec = true;
                        ShowItem();
                    }
                }
                // 아이템 목록이 활성화되어있다면
                else if (itemActivated)
                {
                    // 탭별로 가지고 있는 아이템이 1개 이상이라면 키보드 입력처리
                    if (inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 2)
                                selectedItem += 2;
                            else
                                selectedItem %= 2;
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (selectedItem > 1)
                                selectedItem -= 2;
                            else
                                selectedItem = inventoryTabList.Count - 1 - selectedItem;
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selectedItem > 0)
                                selectedItem--;
                            else
                                selectedItem = inventoryTabList.Count - 1;
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 1)
                                selectedItem++;
                            else
                                selectedItem = 0;
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        // Z키를 누르고 중복처리 되지 않는다면 아이템 사용 및 장착
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            // 소비아이템일 경우 UseCoroutine을 실행하여 아이템 소비 구현
                            if (selectedTab == 0)
                            {
                                StartCoroutine(UseCoroutine("사용", "취소"));
                            }
                            else if (selectedTab == 1)
                            {
                                StartCoroutine(UseCoroutine("장착", "취소"));
                            }
                            else
                            {
                                theAudio.Play(beep_sound);
                            }
                        }
                    }

                    // X키를 누르면 아이템 목록에서 탈출하여 탭 메뉴를 활성화시킴
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        theAudio.Play(cancel_sound);
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                }
                // Z키를 떼는 순간 preventExec를 false로 바꿈
                if (Input.GetKeyUp(KeyCode.Z))
                    preventExec = false;
            }
        }
    }

    // 소모품 사용
    IEnumerator UseCoroutine(string _up, string _down)
    {
        theAudio.Play(enter_sound);
        stopKeyInput = true;

        // 사용할 것인지 말 것인지 확인하는 UI 출력
        go_UseOrCancel.SetActive(true);
        // 사용 확인 UI를 활성화
        theUse.ShowTowChoice(_up, _down);
        // 사용 확인 UI가 끝날때까지 대기
        yield return new WaitUntil(() => !theUse.activated);

        // 사용을 했다면 여기로, 아니라면 실행하지 않음
        if (theUse.GetResult())
        {
            for (int i = 0; i < inventoryItemList.Count; i++)
            {
                // 데이터베이스에 접근해서 현재 가지고 있는 아이템ID와 일치하는 효과를 출력
                theDatabase.UseItem(inventoryItemList[i].itemID);
                // 현재 선택한 아이템과 일치하는 인벤토리에 소지하고 있는 아이템을 찾음
                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    // 소모품이라면
                    if (selectedTab == 0)
                    {
                        // 갯수가 2개 이상이라면 갯수를 하나 줄여주고, 1개라면 아이템을 삭제(0개라면 위 조건문으로 들어오지 않음)
                        if (inventoryItemList[i].itemCount > 1)
                            inventoryItemList[i].itemCount--;
                        else
                            inventoryItemList.RemoveAt(i);
                        // 아이템이 삭제되면 인벤토리가 갱신되어야하기 때문에 ShowItem() 함수 호출
                        ShowItem();
                        break;
                    }
                    // 장비템이라면
                    else if (selectedTab == 1)
                    {
                        // 아이템을 장비창에 추가하고 현재 인벤토리에서 삭제 및 인벤토리 갱신
                        theEquip.EquipItem(inventoryItemList[i]);
                        inventoryItemList.RemoveAt(i);
                        ShowItem();
                        break;
                    }
                }
            }
        }
        // 키 입력을 이제 받아오고 확인 UI를 꺼준다
        stopKeyInput = false;
        go_UseOrCancel.SetActive(false);
    }
}
