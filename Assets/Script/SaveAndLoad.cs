using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 세이브 & 로드 기능을 구현할때 필요한 라이브러리
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveAndLoad : MonoBehaviour
{
    // System.Serializable은 단순히 인스펙터 창에 띄우기 위한 것이 아니라 "직렬화"를 시켜주는 것이다.
    // "직렬화"를 시켜주어야 Binary 파일로 데이터를 저장하고 변환해서 불러올 수 있다.
    [System.Serializable]
    public class Data
    {
        // 캐릭터의 위치 값을 받아오는 변수
        public float playerX;
        public float playerY;
        public float playerZ;

        // 캐릭터의 스탯을 받아오는 변수
        public int playerLv;
        public int playerHP;
        public int playerMP;
        public int playerHPR;
        public int playerMPR;
        public int playerATK;
        public int playerDEF;

        // 캐릭터의 현재 체력, 마나, 경험치
        public int playerCurrentHP;
        public int playerCurrentMP;
        public int playerCurrentEXP;

        // 캐릭터가 장비로 추가된 스탯
        public int added_atk;
        public int added_def;
        public int added_recover_hpr;
        public int added_recover_mpr;

        // 캐릭터의 소지 아이템, 소지 아이템의 갯수, 착용한 장비 아이템 리스트
        public List<int> playerItemInventory;
        public List<int> playerItemInventoryCount;
        public List<int> playerEquipItem;

        // 캐릭터가 현재 있는 맵 이름, 현재 씬 이름
        public string mapName;
        public string sceneName;

        // 게임 내 이벤트 진행 유무, 이벤트 이름
        public List<bool> swList;
        public List<string> swNameList;

        // 게임 내 존재하는 변수들의 이름 및 변수의 값(골드 등)
        public List<string> varNameList;
        public List<float> varNumberList;
    }

    PlayerManager thePlayer;
    PlayerStat thePlayerStat;
    DatabaseManager theDatabase;
    Inventory theInven;
    Equipment theEquip;

    // data 객체를 만들어서 이 객체에 저장해야 하는 요소들을 넣을 것임
    public Data data;

    Vector3 vector;

    public void CallSave()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        theInven = FindObjectOfType<Inventory>();
        theEquip = FindObjectOfType<Equipment>();

        // data 객체에 캐릭터 위치 값 저장
        data.playerX = thePlayer.transform.position.x;
        data.playerY = thePlayer.transform.position.y;
        data.playerZ = thePlayer.transform.position.z;

        // data 객체에 캐릭터 스탯, 현재 상태, 추가된 스택 저장
        data.playerLv = thePlayerStat.character_level;
        data.playerHP = thePlayerStat.hp;
        data.playerMP = thePlayerStat.mp;
        data.playerHPR = thePlayerStat.recovery_Hp;
        data.playerMPR = thePlayerStat.recovery_Mp;
        data.playerATK = thePlayerStat.atk;
        data.playerDEF = thePlayerStat.def;
        data.playerCurrentHP = thePlayerStat.currentHP;
        data.playerCurrentMP = thePlayerStat.currentMP;
        data.playerCurrentEXP = thePlayerStat.currentExp;
        data.added_atk = theEquip.added_atk;
        data.added_def = theEquip.added_def;
        data.added_recover_hpr = theEquip.added_hpr;
        data.added_recover_mpr = theEquip.added_mpr;

        // data 객체에 현재 맵 이름, 현재 씬 이름 저장
        data.mapName = thePlayer.currentMapName;
        data.sceneName = thePlayer.currentSceneName;

        Debug.Log("기초 데이터 성공");

        // 소지 아이템, 소지 아이템 갯수, 장착 아이템들을 초기화함
        // 초기화하지 않으면 세이브&로드를 반복할 때마다 아이템이 복사가 됨
        data.playerItemInventory.Clear();
        data.playerItemInventoryCount.Clear();
        data.playerEquipItem.Clear();

        // data 객체에 변수 이름 및 변수의 값 저장
        for (int i = 0; i < theDatabase.var_name.Length; i++)
        {
            data.varNameList.Add(theDatabase.var_name[i]);
            data.varNumberList.Add(theDatabase.var[i]);
        }

        // data 객체에 이벤트 진행 유무 및 이벤트 이름 저장
        for (int i = 0; i < theDatabase.switch_name.Length; i++)
        {
            data.swNameList.Add(theDatabase.switch_name[i]);
            data.swList.Add(theDatabase.switches[i]);
        }

        // 소지 아이템을 넣을 임시 Item 리스트 생성 후 Inventory의 아이템을 불러옴
        List<Item> itemList = theInven.SaveItem();

        // data 객체에 소지 아이템 및 소지 아이템 갯수 저장
        for (int i = 0; i < itemList.Count; i++)
        {
            Debug.Log("인벤토리의 아이템 저장 완료: " + itemList[i].itemID);
            data.playerItemInventory.Add(itemList[i].itemID);
            data.playerItemInventoryCount.Add(itemList[i].itemCount);
        }

        // data 객체에 장착 아이템 저장
        for (int i = 0; i < theEquip.equipItemList.Length; i++)
        {
            data.playerEquipItem.Add(theEquip.equipItemList[i].itemID);
            Debug.Log("장착된 장비 아이템 저장 완료: " + theEquip.equipItemList[i].itemID);
        }

        // 직렬화된 데이터를 이진 데이터로 변환
        BinaryFormatter bf = new BinaryFormatter();
        // 변환된 파일을 저장함. 게임이 설치된 폴더의 에셋 폴더에 SaveFile.dat을 생성
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat");

        // data의 정보를 file에 기록하고 직렬화를 시켜줌
        bf.Serialize(file, data);
        file.Close();

        Debug.Log(Application.dataPath + "의 위치에 저장했습니다.");
    }

    public void CallLoad()
    {
        // 직렬화된 데이터를 이진 데이터로 변환
        BinaryFormatter bf = new BinaryFormatter();
        // 변환된 파일을 불러옴. 게임이 설치된 폴더의 에셋 폴더에 SaveFile.dat을 FileMode.Open 모드로 불러옴
        FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open);

        // 만약 saveFile이 있고, 그 파일에 저장된 내용이 있다면 Load
        if (file != null && file.Length > 0)
        {
            // Binary 형식으로 변환한 파일을 불러와서 직렬화를 해제하고 Data 타입으로 명시적 형변환을 해줌
            data = (Data)bf.Deserialize(file);

            thePlayer = FindObjectOfType<PlayerManager>();
            thePlayerStat = FindObjectOfType<PlayerStat>();
            theDatabase = FindObjectOfType<DatabaseManager>();
            theInven = FindObjectOfType<Inventory>();
            theEquip = FindObjectOfType<Equipment>();

            // 캐릭터 위치값 로드
            vector.Set(data.playerX, data.playerY, data.playerZ);
            thePlayer.transform.position = vector;

            // 캐릭터 스탯, 캐릭터 현재 상태 로드
            thePlayerStat.character_level = data.playerLv;
            thePlayerStat.hp = data.playerHP;
            thePlayerStat.mp = data.playerMP;
            thePlayerStat.recovery_Hp = data.playerHPR;
            thePlayerStat.recovery_Mp = data.playerMPR;
            thePlayerStat.atk = data.playerATK;
            thePlayerStat.def = data.playerDEF;
            thePlayerStat.currentHP = data.playerCurrentHP;
            thePlayerStat.currentMP = data.playerCurrentMP;
            thePlayerStat.currentExp = data.playerCurrentEXP;

            // 캐릭터 추가 스탯 로드
            theEquip.added_atk = data.added_atk;
            theEquip.added_def = data.added_def;
            theEquip.added_hpr = data.added_recover_hpr;
            theEquip.added_mpr = data.added_recover_mpr;

            // 캐릭터가 존재하는 맵 이름, 저장 시점의 씬이름 로드
            thePlayer.currentMapName = data.mapName;
            thePlayer.currentSceneName = data.sceneName;

            // 변수이름, 변수값, 이벤트 유무, 이벤트 이름 로드
            theDatabase.var = data.varNumberList.ToArray();
            theDatabase.var_name = data.varNameList.ToArray();
            theDatabase.switches = data.swList.ToArray();
            theDatabase.switch_name = data.swNameList.ToArray();

            // 저장된 장착 아이템이 데이터베이스에 존재한다면 장착 아이템 로드
            for (int i = 0; i < theEquip.equipItemList.Length; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerEquipItem[i] == theDatabase.itemList[x].itemID)
                    {
                        theEquip.equipItemList[i] = theDatabase.itemList[x];
                        Debug.Log("장착된 아이템을 로드했습니다." + theEquip.equipItemList[i].itemID);
                        break;
                    }
                }
            }

            // 소지 아이템을 불러올 임시 Item 리스트
            List<Item> itemList = new List<Item>();

            // 저장된 소지 아이템이 데이터베이스에 존재한다면 Item 리스트에 추가
            for (int i = 0; i < data.playerItemInventory.Count; i++)
            {
                for (int x = 0; x < theDatabase.itemList.Count; x++)
                {
                    if (data.playerItemInventory[i] == theDatabase.itemList[x].itemID)
                    {
                        itemList.Add(theDatabase.itemList[x]);
                        Debug.Log("인벤토리 아이템을 로드했습니다." + theDatabase.itemList[x].itemID);
                        break;
                    }
                }
            }

            // 저장된 아이템 갯수만큼 Item 리스트의 itemCount에 로드
            for (int i = 0; i < data.playerItemInventoryCount.Count; i++)
            {
                itemList[i].itemCount = data.playerItemInventoryCount[i];
            }

            // itemList를 인자로 넘겨서 인벤토리를 로드 및 스탯창 출력
            theInven.LoadItem(itemList);
            theEquip.ShowText();

            // 카메라의 전환을 위한 함수 호출
            // 씬이 로드되면 모든 내용이 초기화가 되기 때문에 씬 전환 이후 바로 함수가 호출 될 수 있도록 Coroutine으로 작성함
            GameManager theGM = FindObjectOfType<GameManager>();
            theGM.LoadStart();
            SceneManager.LoadScene(data.sceneName);
        }
        else
        {
            Debug.Log("저장된 세이브 파일이 없습니다.");
        }

        file.Close();
    }
}
