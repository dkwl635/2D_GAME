using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;
public class BossMonster : MonoBehaviour, ITakeDamage
{
 public   enum Monster_State //보스 몬스터 상태
    {
        Idle,
        Move,
        Attack1,
        Attack2,
        Die
    }


    HeroCtrl targetHero;
    Transform targetTr; //타겟 : 플레이어 
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidbody;

    [Header("MonsterStatus")]
    public float speed = 2.0f; //이동 속도
    public int hp = 30;
    public int attackPower = 10;
    public float attakcDis = 4;


    public Transform damageTxtPos;
    public LayerMask heroLayer;
    
    Vector3 targetToThis = Vector3.zero; //타겟과의 거리를 구하기 위해
    Vector3 dir = Vector3.zero; // 방향

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
        targetToThis = targetTr.position - transform.position; //타겟과의 거리관계
        dir = targetToThis.normalized;     //방향값

        float dis = targetToThis.sqrMagnitude;  //거리 길이 변환
      
        //길이에 따른 상태 변환
        if (dis > attakcDis)
            MonsterState_Update(Monster_State.Move);
        else if (dis <= attakcDis)
            MonsterState_Update(Monster_State.Attack1);

        Flip_Update();//이미지 좌우변환


        spriteRenderer.sortingOrder = -1 * (int)transform.position.y;
    }

    void MonsterState_Update(Monster_State newStatue) //몬스터의 상태 변화에 따른 애니메이터 변환해주기
    {
        if (monster_State.Equals(newStatue)) return; //이전과 같은 상태면 리턴

        if (monster_State.Equals(Monster_State.Die)) return; //죽은 상태면

        //새로운 상태 적용
        monster_State = newStatue;
        //속도 초기화
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
    void Flip_Update() //이미지 좌우 변환
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
            Debug.Log("플레이어 공격");
        }
    }

    public void TakeDamage(int value)
    {
        //if (hp <= 0)
        //    return;

        hp -= value;
        //데미지 이펙트
        GameMgr.Inst.DamageTxtEffect.GetObj().SetDamageTxt(value, damageTxtPos.position);

        ////공격중이고 죽을체력이아니면
        //if (monster_State.Equals(Monster_State.Attack) && hp > 0)
        //    return;

        ////애니메이션 실행 ... 이미 애니메이션 실행중이면
        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        //    animator.SetTrigger("Hit");

        //if (hp <= 0)
        //    MonsterState_Update(Monster_State.Die);
    }
}
