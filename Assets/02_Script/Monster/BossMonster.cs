using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;
using CardHelp;

public class BossMonster : MonoBehaviour, ITakeDamage ,SetCard
{
    public enum Monster_State //���� ���� ����
    {
        Idle,
        Move,
        Attack1,
        Attack2,
        Die
    }


    [HideInInspector] public HeroCtrl targetHero;
    protected Transform targetTr; //Ÿ�� : �÷��̾� 
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected Rigidbody2D rigidbody;
    public Collider2D collider;

    [Header("MonsterStatus")]
    public Sprite monsterImg;
    public float speed = 2.0f; //�̵� �ӵ�
    public int hp = 30;
    public int attackPower = 10;
    public float attakcDis = 4;



    public Transform damageTxtPos;
    public LayerMask heroLayer;

    protected  Vector3 targetToThis = Vector3.zero; //Ÿ�ٰ��� �Ÿ��� ���ϱ� ����
    protected  Vector3 dir = Vector3.zero; // ����
    protected float dis;

    public Monster_State monster_State;

 
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    protected virtual  void Start()
    {
        targetHero = GameMgr.Inst.hero;
        targetTr = targetHero.transform;

    }

    protected virtual void Update()
    {
        targetToThis = targetTr.position - transform.position; //Ÿ�ٰ��� �Ÿ�����
        dir = targetToThis.normalized;     //���Ⱚ
        dis = targetToThis.sqrMagnitude;  //�Ÿ� ���� ��ȯ

        Flip_Update();//�̹��� �¿캯ȯ
      
    }


    protected virtual void  MonsterState_Update(Monster_State newStatue) //������ ���� ��ȭ�� ���� �ִϸ����� ��ȯ���ֱ�
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
            case Monster_State.Attack2:
                {
                    animator.SetBool("Move", false);
                    animator.SetTrigger("Attack02");
               
                }
                break;
            case Monster_State.Die:
                {
                    animator.speed = 0.0f;
                    StartCoroutine(Die_Co());
                }
                break;
            default:
                break;
        }


    }
  protected void Flip_Update() //�̹��� �¿� ��ȯ
    {
        if (targetToThis.x < 0)
            spriteRenderer.flipX = true;
        else if (targetToThis.x > 0)
            spriteRenderer.flipX = false;

        //���Ʒ� ������ ����
        spriteRenderer.sortingOrder = -1 * (int)transform.position.y;
    }



    public virtual void TakeDamage(int value)
    {   
        hp -= value;
        //������ ����Ʈ
        GameMgr.Inst.DamageTxtEffect.GetObj().SetDamageTxt(value, damageTxtPos.position);

        if (hp <= 0)
            MonsterState_Update(Monster_State.Die);
    }

  protected   IEnumerator Die_Co()
    {
        GameMgr.Inst.BossKill();
        //����ġ�� ����
        for (int i = 0; i < 5; i++)
        {
            GameMgr.Inst.SpawnExpBall(transform.position, 2);
        }
        //���� ����
        for (int i = 0; i < 10; i++)
        {
            if (Random.Range(0, 3) == 0)
                GameMgr.Inst.SpawnCoin(transform.position);
        }
     

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

        Destroy(this.gameObject);
    }

    public CardData GetCard()
    {
        CardData card;
        card.img = monsterImg;
        card.info = "Hp : " + hp + "\nAttack : " + attackPower;
        return card;
    }
}
