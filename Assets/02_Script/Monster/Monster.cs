using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;

public class Monster : MonoBehaviour , ITakeDamage
{
    HeroCtrl targetHero; //Ÿ�� Ŭ����
    Transform targetTr; //Ÿ�� : �÷��̾� 
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidbody;  //HIT �� �˹��� �����ϰ��� �־�Ҵ�.
    CircleCollider2D collider;

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

    //���ͺ� �ִϸ����� �����ϱ� ����
    RuntimeAnimatorController runtimeAnimatorController;


    private void Awake()
    {
        //�ʱ�ȭ
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();  
    }

    private void OnEnable()
    {//Ȱ��ȭ��       
        targetHero = GameMgr.Inst.hero;
        targetTr = GameMgr.Inst.hero.transform;
        //��ϵ� �ִϸ����� ��Ʈ�ѷ� �ٲٱ�
        animator.runtimeAnimatorController = runtimeAnimatorController;
        collider.enabled = true;
    }

    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {//�̵� �ִϸ��̼��϶��� �̵������ϰ� �ϱ� ����
            rigidbody.velocity = Vector2.zero; //�ӵ����� �ѹ� �ʱ�ȭ
            rigidbody.velocity = dir * speed; //�̵�    
        }

    }

    private void Update()
    {
        //����� �Ÿ� ���
        targetToThis = targetTr.position - transform.position; //Ÿ�ٰ��� �Ÿ�����
        dir = targetToThis.normalized;     //���Ⱚ
        float dis = targetToThis.sqrMagnitude;  //�Ÿ� ���� ��ȯ
        //���̿� ���� ���� ��ȯ
        if (dis > attakcDis)
            MonsterState_Update(Monster_State.Move);
        else if (dis <= attakcDis)
            MonsterState_Update(Monster_State.Attack);

        Flip_Update();//�̹��� �¿캯ȯ
    }

    public void SetStatus(MonsterData monsterData) 
    {//���� �����͸� ������ ����
        runtimeAnimatorController = monsterData.monsterAnimator; //�ִϸ�����
        collider.offset = monsterData.offset;   //��ü �ݶ��̴� 
        collider.radius = monsterData.coliderSize;  //������
        //���ݹڽ�
        attackSize = monsterData.attackBoxSize; 
        //�ɷ�ġ
        attackPower = monsterData.AttPw;   
        this.hp = monsterData.hp;
        speed = monsterData.Speed;
    
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

        switch (monster_State)//���º� �ִϸ��̼� 
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
        //�̹��� ���Ʒ� ����
        spriteRenderer.sortingOrder = -1 * (int)transform.position.y;
    }

  public void TakeDamage(int value = 10) //�������� �޴� �Լ�
    {
        if (hp <= 0)
            return;

        hp -= value;
        rigidbody.velocity = Vector2.zero;
        //������ ����Ʈ
        GameMgr.Inst.DamageTxtEffect_P.GetObj().SetDamageTxt(value, damageTxtPos.position);

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
        //���� ����
        if(Random.Range(0, 2) == 0)
            GameMgr.Inst.SpawnCoin(transform.position);

        //�˹� Ÿ�ٰ��� �ݴ� ��������
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(targetToThis.normalized * -1 * 300.0f);

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
        Collider2D hit = Physics2D.OverlapBox(transform.position + targetToThis.normalized, attackSize, 0, heroLayer);
        if (hit && hit.CompareTag("Player"))
        {
            //hit.SendMessage("TakeDamage", attackPower);      
            targetHero.TakeDamage(attackPower);
        }

    }

   
}
