using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;

public class GuidedSword_Collider : SkillDamageCollider
{
    public delegate void GuidedSword_Event(GuidedSword_Collider guidedSword_Collider);
    public GuidedSword_Event DeQuObj; //�ٽ� ����Ҽ� �ֵ��� �������� ����

    public SpriteRenderer img; //����Į �̹���
    bool move = false;            //�����̴��� 

    public GameObject target;  //Ÿ��
    public float speed = 10.0f; //���ư��� �ӵ�
    Transform originPos;          //���ƿ;��ϴ� ��ġ
    Vector2 dir = Vector2.zero; //����   
    private void Update()
    {
        if (move)
        {   //Ÿ���� Ȱ��ȭ �ȴٸ�
            if (target.activeSelf)//������ ���Ѵ�
                dir = (target.transform.position - transform.position).normalized;
            else//��Ȱ��ȭ ���� ��� ����Į �������
                gameObject.SetActive(false);

            //�̵� �� ����
            transform.position += (Vector3)dir * Time.deltaTime * speed;     
            transform.up = dir; //Į �Ӹ��� �̵������� ���� ���ư���.

        }

    }
    private void OnEnable()
    {
        //���� ��ġ ���
        transform.position = originPos.position; 
        transform.rotation = Quaternion.identity;      
        //�߻������� ���
        collider2D.enabled = false;
        move = false;
    }
    private void OnDisable()
    { //�����̰� �ִµ��� �������
        if (move)
        {
            move = false;
            DeQuObj?.Invoke(this); //�ٽ� ��ų��ü�� �����ֱ�
        }
       
    }
    public void SetTarget(GameObject target)
    {//Ÿ�� ������ �����̱� ����
        this.target = target;
        move = true;   
        collider2D.enabled = true;
    }
    
    public void InitSword(Transform origin)
    {//ó�� Į ���� 
        originPos = origin; //���ƿ��� ��ġ
        transform.position = origin.position;
    }

    public void SetSword(Sprite sprite, float speed)
    {//������ �̹����� ���ư��� �ӵ� ������ ����
        img.sprite = sprite;
        this.speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")) //���Ϳ� �浹��
        {//������ �ֱ�
            OnTriggerMonster?.Invoke(collision.GetComponent<ITakeDamage>());          
            target = null;
            gameObject.SetActive(false);
        }

    }

 

}
