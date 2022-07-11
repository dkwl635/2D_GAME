using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCtrl : MonoBehaviour
{
    Transform tr;
    Transform heroModel;
    Animator animator;

    Vector3 mvDir = Vector3.zero;
  
    [Header("Move")]
    [SerializeField] private float speed = 2;
  
    private Vector3 originScale;
    [Header("Attack")]
    public GameObject attackPoint;
    public int AttackPower = 10;


    private void Awake()
    {
        tr = GetComponent<Transform>();
        heroModel = tr.GetChild(0);
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        originScale = heroModel.localScale;
    }

    private void FixedUpdate()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        tr.position += mvDir * Time.fixedDeltaTime * speed;
    }

    public void SetJoyStickMv(Vector3 dir, bool sprint = false)
    {
        mvDir = dir; //�̵� ���� ����

        //�ִϸ����� ����
        if (mvDir.Equals(Vector3.zero)) animator.SetBool("move", false);
        else animator.SetBool("move", true);

        //�޸��� ����
        if (sprint)
        {
            animator.speed = 1.2f;
            speed = 4.0f;
        }
        else
        {
            animator.speed = 1.0f;
            speed = 2.0f;
        }

        //�̹��� �¿� ����
        if (mvDir.x < 0)
            heroModel.localScale = originScale;
        else if(mvDir.x > 0)
        {
            Vector3 temp = originScale;
            temp.x *= -1;
            heroModel.localScale = temp;
        }
    }

    public void Attack()
    {
        //���� �������̸� ����
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        animator.SetTrigger("Attack");   
    }

    public void Attack_Event()
    {
        //���������� �߽ɿ��� �׸� ũ�� ��ŭ ���� �浿�� �ݶ��̴� ��������
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.transform.position, new Vector2(2, 2), 0);
        
        for (int i = 0; i < hits.Length; i++)            //������ �ֱ� 
            hits[i].SendMessage("TakeDamage", AttackPower);
        
    }

}
