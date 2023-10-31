using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    // 몬스터 공격시 데미지를 띄울 텍스트와 캔버스를 담을 오브젝트
    public GameObject prefabs_Floating_Text;
    public GameObject parent;

    // 플레이어 공격 사운드
    public string atkSound;

    // 플레이어의 공격력을 받아오기 위함
    PlayerStat thePlayerStat;
    // Start is called before the first frame update
    void Start()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어의 공격모션이 적에게 닿았다면
        if (other.gameObject.tag == "Enemy")
        {
            // Hit함수를 호출하고 입힌 데미지의 값을 받아와서 플로팅 텍스트에 출력한다.
            int dmg = other.gameObject.GetComponent<EnemyStat>().Hit(thePlayerStat.atk);
            AudioManager.instance.Play(atkSound);

            // 플로팅 텍스트를 위한 Vector값 조절 및 텍스트, 컬러, 폰트사이즈 조절
            Vector3 vector = other.transform.position;
            vector.y += 60;

            GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingText>().text.text = dmg.ToString();
            clone.GetComponent<FloatingText>().text.color = Color.white;
            clone.GetComponent<FloatingText>().text.fontSize = 25;
            clone.transform.SetParent(parent.transform);
        }
    }
}
