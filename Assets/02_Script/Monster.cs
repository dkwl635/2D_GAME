using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform targetTr; //Ÿ�� : �÷��̾� 
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody;  //HIT �� �˹��� �����ϰ��� �־�Ҵ�.

    enum Monster_State //���� ����
    {
        Idle,
        Move,
        Attack,
        Die
    }

    [SerializeField] Monster_State monster_State = Monster_State.Idle;
    [SerializeField] float attakcDis = 2.0f;    //���� ��Ÿ�

    [Header("MonsterStatus")]
    float speed = 1.0f; //�̵� �ӵ�
    public int hp = 30;

    public Transform damageTxtPos;

    Vector3 targetToThis = Vector3.zero; //Ÿ�ٰ��� �Ÿ��� ���ϱ� ����

    private void Awake()
    {
        //�ʱ�ȭ
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (monster_State.Equals(Monster_State.Move)) //�̵� ����
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) return; //�̵� �ִϸ��̼��϶��� �̵������ϰ� �ϱ� ����

            rigidbody.velocity = Vector2.zero; //�ӵ����� �ʿ䰡���⶧����
            Vector3 dir = targetToThis.normalized;     //���Ⱚ
            transform.position += dir * Time.fixedDeltaTime * speed; //�̵�
        }
    }

    private void Update()
    {
        targetToThis = targetTr.position - transform.position; //Ÿ�ٰ��� �Ÿ�����

        float dis = targetToThis.sqrMagnitude;  //�Ÿ� ���� ��ȯ
        //���̿� ���� ���� ��ȯ
        if (dis > attakcDis)
            MonsterState_Update(Monster_State.Move);
        else if (dis <= attakcDis)
            MonsterState_Update(Monster_State.Attack);

        Flip_Update();//�̹��� �¿캯ȯ

    }


    void MonsterState_Update(Monster_State newStatue) //������ ���� ��ȭ�� ���� �ִϸ����� ��ȯ���ֱ�
    {
        if (monster_State.Equals(newStatue)) return; //������ ���� ���¸� ����

        if (monster_State.Equals(Monster_State.Die)) return; //���� ���¸�

        //���ο� ���� ����
        monster_State = newStatue;

        switch (monster_State)
        {
            case Monster_State.Idle:
                animator.SetBool("Move", false);
                break;
            case Monster_State.Move:
                animator.SetBool("Move", true);
                break;
            case Monster_State.Attack:
                {
                    animator.SetBool("Move", false);
                    animator.SetTrigger("Attack");
                }
                break;
            case Monster_State.Die:
                {
                    animator.SetBool("Move", false);
                    animator.SetTrigger("Death");
                }
                break;
            default:
                break;
        }


    }

    void Flip_Update() //�̹��� �¿� ��ȯ
    {
        if (targetToThis.x < 0)
            spriteRenderer.flipX = true;
        else if (targetToThis.x > 0)
            spriteRenderer.flipX = false;
    }

    void TakeDamage(int value = 10) //�������� �޴� �Լ�
    {
        if (hp <= 0)
            return;

        //������ ����Ʈ
        GameMgr.Inst.DamageTxt_Stack.GetObj().SetDamageTxt(value, damageTxtPos.position);


        //�˹� Ÿ�ٰ��� �ݴ� ��������
        rigidbody.AddForce(targetToThis.normalized * -1 * 10.0f);

        //�ִϸ��̼� ���� ... �̹� �ִϸ��̼� �������̸� 
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            animator.SetTrigger("Hit");

        hp -= value;
        if (hp <= 0)
        {
            MonsterState_Update(Monster_State.Die);
        }

    }

    //�ִϸ��̼ǿ� ������ DIE �̺�Ʈ
    public void Die_Event()
    {
        gameObject.SetActive(false);
    }


}
