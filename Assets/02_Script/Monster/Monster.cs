using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    HeroCtrl targetHero;
    Transform targetTr; //Ÿ�� : �÷��̾� 
    public SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidbody;  //HIT �� �˹��� �����ϰ��� �־�Ҵ�.
    Collider2D collider;

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
    public float speed = 2.0f; //�̵� �ӵ�
    public int hp = 30;
    public int attackPower = 10;
    public Vector2 attackSize = Vector2.zero;
    public Transform damageTxtPos;




    public LayerMask heroLayer;
    Vector3 targetToThis = Vector3.zero; //Ÿ�ٰ��� �Ÿ��� ���ϱ� ����
    Vector3 dir = Vector3.zero; // ����

    private void Awake()
    {
        //�ʱ�ȭ
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        targetHero = GameMgr.Inst.hero;
        targetTr = GameMgr.Inst.hero.transform;
        collider.enabled = true;
    }

    private void FixedUpdate()
    {    
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) //�̵� �ִϸ��̼��϶��� �̵������ϰ� �ϱ� ����
            {
                rigidbody.velocity = Vector2.zero; //�ӵ����� �ѹ� �ʱ�ȭ
                rigidbody.velocity = dir * speed; //�̵�    
            }
   
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
            MonsterState_Update(Monster_State.Attack);

        Flip_Update();//�̹��� �¿캯ȯ


        spriteRenderer.sortingOrder = -1 * (int)transform.position.y;

      


    }

    public void SetStatus(int hp)
    {
        this.hp = hp;
        monster_State = Monster_State.Idle;     
        gameObject.SetActive(true);
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
                    StartCoroutine(Die_Co());
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

  public  void TakeDamage(int value = 10) //�������� �޴� �Լ�
    {
        if (hp <= 0)
            return;

        hp -= value;
        rigidbody.velocity = Vector2.zero;
        //������ ����Ʈ
        GameMgr.Inst.DamageTxtEffect.GetObj().SetDamageTxt(value, damageTxtPos.position);

        //�������̰� ����ü���̾ƴϸ�
        if (monster_State.Equals(Monster_State.Attack) && hp > 0)
            return;

        //�ִϸ��̼� ���� ... �̹� �ִϸ��̼� �������̸�
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))             
            animator.SetTrigger("Hit"); 
        
          if (hp <= 0)    
            MonsterState_Update(Monster_State.Die);
        

    }
    IEnumerator Die_Co()
    {
        //���Ӹ޴��� 
        GameMgr.Inst.MonsterKill();
        //����ġ�� ����
        GameMgr.Inst.SpawnExpBall(transform.position, 2);

        //�˹� Ÿ�ٰ��� �ݴ� ��������
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(targetToThis.normalized * -1 * 3000.0f);

        float a = 1;
        Color color = Color.white;
        collider.enabled = false;

        while (a > 0) //���������鼭 �������
        {
            yield return null;
            a -= Time.deltaTime;
            color.a = a;
            spriteRenderer.color = color;
       }

        Die_Event();
    }

    //DIE �̺�Ʈ
    public void Die_Event()
    {

        gameObject.SetActive(false);
        spriteRenderer.color = Color.white;
        GameMgr.Inst.monsters_P.ReturnObj(this);
    }

    public void Attack_Event()
    { 
        //���������� �߽ɿ��� �׸� ũ�� ��ŭ ���� �浿�� �ݶ��̴� ��������
        Collider2D hit = Physics2D.OverlapBox(transform.position +  targetToThis.normalized , attackSize, 0, heroLayer) ;
        if(hit && hit.CompareTag("Player"))
        {
            //hit.SendMessage("TakeDamage", attackPower);      
            targetHero.TakeDamage(attackPower);            
            GameMgr.Inst.playerHitEffect_P.GetObj().SetEffect(hit.transform.position , HitType.nomarl);
        }

       
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + targetToThis.normalized, attackSize);
    }

}
