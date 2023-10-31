using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // 다른 스크립트에서 사용하게 하기 위한 instance
    public static PlayerStat instance;

    // 플레이어의 최대체력
    public int hp;
    // 플레이어의 현재체력
    public int currentHP;
    // 플레이어의 최대마나
    public int mp;
    // 플레이어의 현재마나
    public int currentMP;
    // 플레이어의 현재 레벨
    public int character_level;
    // 플레이어가 레벨업당 필요한 경험치
    public int[] needExp;
    // 플레이어의 현재 경험치
    public int currentExp;
    // 플레이어의 공격력
    public int atk;
    // 플레이어의 방어력
    public int def;
    // 플레이어의 피격사운드
    public string dmgSound;
    // 데미지 관련 수치를 나타낼 텍스트 오브젝트
    public GameObject prefab_Floating_text;
    // 플로팅 텍스트는 UI기때문에 Canvas 바로 밑에 자식으로 생성되게 하기 위해서 Canvas를 담을 오브젝트
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 플레이어 피격시 실행 함수
    public void Hit(int _enemyAtk)
    {
        // 방어력이 적의 공격력보다 높다면 데미지를 1로, 공격력보다 낮다면 공격력에서 방어력을 뺀 수치만큼 데미지를 입음
        int dmg;
        if (def >= _enemyAtk)
            dmg = 1;
        else
            dmg = _enemyAtk - def;
        currentHP -= dmg;

        // 현재 체력이 0 이하가 되면 캐릭터 사망
        if (currentHP <= 0)
            Debug.Log("체력 0 미만, 게임 오버");

        // 플레이어 피격 사운드
        AudioManager.instance.Play(dmgSound);

        // 플로팅 텍스트를 위한 Vector값 조절 및 텍스트, 컬러, 폰트사이즈 조절
        Vector3 vector = this.transform.position;
        vector.y += 60;

        GameObject clone = Instantiate(prefab_Floating_text, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = dmg.ToString();
        clone.GetComponent<FloatingText>().text.color = Color.red;
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);

        // 중복 방지를 위해 모든 코루틴을 꺼주고 HitCoroutine 실행
        StopAllCoroutines();
        StartCoroutine(HitCoroutine());

    }

    // 캐릭터가 피격 당하면 깜빡깜빡 거리는 효과를 주는 Coroutine
    IEnumerator HitCoroutine()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }
}
