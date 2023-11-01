using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStat : MonoBehaviour
{
    // 적의 hp, 현재 hp, 공격력, 방어력, 경험치 필드
    public int hp;
    public int currentHp;
    public int atk;
    public int def;
    public int exp;

    public GameObject healthBarBackground;
    public Image healthBarFilled;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        healthBarFilled.fillAmount = 1f;
    }

    // 몬스터가 피격 당했을 때
    public int Hit(int _playerAtk)
    {
        // 몬스터가 피격시 공식에 의해 몬스터에게 데미지를 주고 hp가 0이되면 player에게 경험치를 준다.
        // 이후 데미지를 리턴하는데 이 데미지는 플로팅 텍스트를 띄우기 위한 값이다.
        int playerAtk = _playerAtk;
        int dmg;
        if (def >= playerAtk)
            dmg = 1;
        else
            dmg = playerAtk - def;

        currentHp -= dmg;

        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
            PlayerStat.instance.currentExp += exp;
            // 이 부분에서 레벨업 검사 함수를 작성해도 괜찮을듯? 플레이어의 레벨업 수단이 몬스터 처치 밖에 없다면
        }

        // 몬스터를 때리면 몬스터의 체력바를 깎고 체력바를 띄워줌
        healthBarFilled.fillAmount = (float)currentHp / hp;
        healthBarBackground.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(WaitCoroutine());
        return dmg;
    }

    // 3초 동안 몬스터를 공격하지 않았다면 몬스터의 체력바 비활성화
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(3f);
        healthBarBackground.SetActive(false);
    }
}
