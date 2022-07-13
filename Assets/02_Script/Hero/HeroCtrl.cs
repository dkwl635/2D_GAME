using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeroCtrl : MonoBehaviour
{
    Transform tr;
    Transform heroModel;
    Animator animator;
    Rigidbody2D rigidbody;
    HeroCtrlMgr HeroCtrlMgr; //UI��
    SortingGroup sortingGroup;

    Vector3 mvDir = Vector3.zero;
  
    [Header("Move")]
    [SerializeField] private float speed = 2;
  
    private Vector3 originScale;
    [Header("Attack")]
    public GameObject attackPoint;
    public int AttackPower = 10;

    public int hp = 1000;
    public int maxHp = 1000;


    private void Awake()
    {
        tr = transform;
        heroModel = tr.GetChild(0);
        animator = GetComponentInChildren<Animator>();
        HeroCtrlMgr = GetComponent<HeroCtrlMgr>();
        rigidbody = GetComponent<Rigidbody2D>();
        sortingGroup = GetComponentInChildren<SortingGroup>();
    }

    private void Start()
    {
        originScale = heroModel.localScale;
    }

    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            rigidbody.velocity = mvDir * speed;
        else
            rigidbody.velocity = Vector2.zero;
    }

    private void Update()
    {
        sortingGroup.sortingOrder = -1 * (int)tr.position.y;
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

    public void TakeDamage(int value)
    {
     
        hp -= value;
        HeroCtrlMgr.SetHpImg(hp, maxHp);
    }
}
