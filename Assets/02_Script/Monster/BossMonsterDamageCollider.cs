using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterDamageCollider : MonoBehaviour
{  //�������� ���� ���� �ݶ��̴�

    public delegate void PlayerHit();
    public event PlayerHit TakeDamge;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {          
            TakeDamge(); //��ϵ� ȿ�� ���
            gameObject.SetActive(false); //1ȸ��
        }
    }
}
