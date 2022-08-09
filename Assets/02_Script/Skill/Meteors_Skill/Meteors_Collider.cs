using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;

public class Meteors_Collider : SkillDamageCollider
{
   public AudioSource audioSource; 
    Vector2 dir = new Vector2(1.0f, -1.0f).normalized; //�밢�� ����
    bool move = false;   //�����̴� ��
    private void Update()
    {
        if (move)//�̵� �����ϸ� �밢������ �������� ��ó�� �̵�
            transform.position += (Vector3)dir * Time.deltaTime * 10.0f;
    }

    private void OnEnable()
    {
        //�ݶ��̴��� ���� �̵�
        collider2D.enabled = false;
        move = true;
    }

    public void Meteors_Evenet()
    {//���׿� �ִϸ��̼ǿ� ���߾� ������ ����
        //�ݶ��̴��� Ű�� �������� ���´�.
        audioSource.Play();
        collider2D.enabled = true;
        move = false;
    }
   public void MeteorsEnd_Evenet()
    {//�ִϸ��̼� ����� ������Ʈ ����
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")) //���Ϳ��� ������ ����
            OnTriggerMonster?.Invoke(collision.GetComponent<ITakeDamage>());
    }
}
