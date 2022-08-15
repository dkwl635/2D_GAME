using MonsterHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark_Collider : SkillDamageCollider
{
    Vector2 dir; //����
    Animator animator; //�ִϸ��̼� ������ ����
    float lifeTime = 3.0f; //�����ð�
    float speed = 10.0f; //���ư��� �ӵ�

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    private void OnEnable() //����ũ �߻�
    {
        collider2D.enabled = true; //�ݶ��̴� on
        speed = 10.0f;                  //�ӵ� 
        lifeTime = 3.0f;                //3���� ����
    }
    public void SetSpark(Vector2 dir)
    {
        this.dir = dir;
        transform.right = dir; //���⿡���� ȸ��
        gameObject.SetActive(true);
    }

    private void Update()
    {      
        //�����̱�
        transform.position += (Vector3)dir * Time.deltaTime * speed;
        
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")) //�浹��
        {
            //������ ������
            OnTriggerMonster?.Invoke(collision.GetComponent<ITakeDamage>());
            //�ݶ��̴� off
            collider2D.enabled = false;
            //�ִϸ��̼� ��Ʈ �����ִϸ��̼� ����
            animator.SetTrigger("Hit");
            speed = 0.0f;
            lifeTime = 0.5f;
        }
    }

}
