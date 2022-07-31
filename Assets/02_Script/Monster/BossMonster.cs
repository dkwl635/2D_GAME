using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;
public class BossMonster : MonoBehaviour, ITakeDamage
{
 public   enum Monster_State //���� ���� ����
    {
        Idle,
        Move,
        Attack1,
        Attack2,
        Die
    }


    HeroCtrl targetHero;
    Transform targetTr; //Ÿ�� : �÷��̾� 
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidbody;

    [Header("MonsterStatus")]
    public float speed = 2.0f; //�̵� �ӵ�
    public int hp = 30;
    public int attackPower = 10;
    public float attakcDis = 4;


    public Transform damageTxtPos;
    public LayerMask heroLayer;
    
    Vector3 targetToThis = Vector3.zero; //Ÿ�ٰ��� �Ÿ��� ���ϱ� ����
    Vector3 dir = Vector3.zero; // ����

    public Monster_State monster_State;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();


    }

    private void Start()
    {
        targetTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
      
        rigidbody.MovePosition(transform.position   + targetToThis.normalized * speed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        targetToThis = targetTr.position - transform.position; //Ÿ�ٰ��� �Ÿ�����
        dir = targetToThis.normalized;     //���Ⱚ

        float dis = targetToThis.sqrMagnitude;  //�Ÿ� ���� ��ȯ
      
        //���̿� ���� ���� ��ȯ
        if (dis > attakcDis)
            MonsterState_Update(Monster_State.Move);
        else if (dis <= attakcDis)
            MonsterState_Update(Monster_State.Attack1);

        Flip_Update();//�̹��� �¿캯ȯ


        spriteRenderer.sortingOrder = -1 * (int)transform.position.y;
    }

    void MonsterState_Update(Monster_State newStatue) //������ ���� ��ȭ�� ���� �ִϸ����� ��ȯ���ֱ�
    {
        if (monster_State.Equals(newStatue)) return; //������ ���� ���¸� ����

        if (monster_State.Equals(Monster_State.Die)) return; //���� ���¸�

        //���ο� ���� ����
        monster_State = newStatue;
        //�ӵ� �ʱ�ȭ
        rigidbody.velocity = Vector2.zero;

        switch (monster_State)
        {
            case Monster_State.Idle:
                animator.SetBool("Move", false);

                break;
            case Monster_State.Move:
                animator.SetBool("Move", true);
                break;
            case Monster_State.Attack1:
                {
                    animator.SetBool("Move", false);
                    animator.SetTrigger("Attack01");       
                }
                break;
            case Monster_State.Die:
                {
                    animator.SetBool("Move", false);
                    animator.SetTrigger("Death");
                    //StartCoroutine(Die_Co());
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

    public void Atk01_End()
    {
        MonsterState_Update(Monster_State.Idle);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ����");
        }
    }

    public void TakeDamage(int value)
    {
        //if (hp <= 0)
        //    return;

        hp -= value;
        //������ ����Ʈ
        GameMgr.Inst.DamageTxtEffect.GetObj().SetDamageTxt(value, damageTxtPos.position);

        ////�������̰� ����ü���̾ƴϸ�
        //if (monster_State.Equals(Monster_State.Attack) && hp > 0)
        //    return;

        ////�ִϸ��̼� ���� ... �̹� �ִϸ��̼� �������̸�
        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        //    animator.SetTrigger("Hit");

        //if (hp <= 0)
        //    MonsterState_Update(Monster_State.Die);
    }
}
