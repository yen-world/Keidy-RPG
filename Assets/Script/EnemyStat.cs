using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    // 적의 hp, 현재 hp, 공격력, 방어력, 경험치 필드
    public int hp;
    public int currentHp;
    public int atk;
    public int def;
    public int exp;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
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
        return dmg;
    }
}
