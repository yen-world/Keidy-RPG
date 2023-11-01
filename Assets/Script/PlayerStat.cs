using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // 단위시간당 체력, 마나 재생
    public int recovery_Hp;
    public int recovery_Mp;

    // 단위시간을 정해줄 변수(1초, 2초, 3초 등)
    public float time;
    float current_time;

    // 데미지 관련 수치를 나타낼 텍스트 오브젝트
    public GameObject prefab_Floating_text;
    // 플로팅 텍스트는 UI기때문에 Canvas 바로 밑에 자식으로 생성되게 하기 위해서 Canvas를 담을 오브젝트
    public GameObject parent;

    // 플레이어 체력, 마나 HUD UI
    public Slider hpSlider;
    public Slider mpSlider;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentHP = hp;
        currentMP = mp;
        current_time = time;
    }

    // Update is called once per frame
    void Update()
    {
        // 체력, 마나바의 최대값을 최대체력으로 설정
        hpSlider.maxValue = hp;
        mpSlider.maxValue = mp;

        // 현재 체력, 마나바의 값을 현재 체력, 마나 값으로 설정
        hpSlider.value = currentHP;
        mpSlider.value = currentMP;

        // 현재 경험치가 필요한 경험치량을 채웠다면
        if (currentExp >= needExp[character_level])
        {
            // 남은 경험치는 다음 레벨의 필요한 경험치로 이전시키고 레벨, 체력, 마나를 증가시키고, 현재 체력과 마나를 채워주고 공격력과 방어력도 올려준다.
            currentExp -= needExp[character_level];
            character_level++;
            hp += character_level * 2;
            mp += character_level + 2;

            currentHP = hp;
            currentMP = mp;
            atk++;
            def++;
        }
        // current_time을 매 프레임마다 감소시켜서 0 이하가 되면 체력 재생이 이루어지게함.
        current_time -= Time.deltaTime;

        if (current_time <= 0)
        {
            if (recovery_Hp > 0)
            {
                if (currentHP + recovery_Hp <= hp)
                    currentHP += recovery_Hp;
                else
                    currentHP = hp;
            }
            current_time = time;
        }
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
