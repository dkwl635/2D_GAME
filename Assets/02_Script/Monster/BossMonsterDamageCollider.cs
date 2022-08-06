using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterDamageCollider : MonoBehaviour
{  //보스몬스터 전용 공격 콜라이더

    public delegate void PlayerHit();
    public event PlayerHit TakeDamge;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {          
            TakeDamge(); //등록된 효과 사용
            gameObject.SetActive(false); //1회용
        }
    }
}
