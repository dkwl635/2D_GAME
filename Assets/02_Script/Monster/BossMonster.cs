using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;
public class BossMonster : MonoBehaviour, ITakeDamage
{
    public enum Monster_State //보스 몬스터 상태
    {
        Idle,
        Move,
        Attack1,
        Attack2,
        Die
    }


    protected HeroCtrl targetHero;
    protected Transform targetTr; //타겟 : 플레이어 
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected Rigidbody2D rigidbody;
    public Collider2D collider;

    [Header("MonsterStatus")]
    public float speed = 2.0f; //이동 속도
    public int hp = 30;
    public int attackPower = 10;
    public float attakcDis = 4;


    public Transform damageTxtPos;
    public LayerMask heroLayer;

    protected  Vector3 targetToThis = Vector3.zero; //타겟과의 거리를 구하기 위해
    protected  Vector3 dir = Vector3.zero; // 방향

    public Monster_State monster_State;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        targetHero = GameObject.FindObjectOfType<HeroCtrl>();
        targetTr = targetHero.transform;

    }


 

    protected virtual void  MonsterState_Update(Monster_State newStatue) //몬스터의 상태 변화에 따른 애니메이터 변환해주기
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
  protected   void Flip_Update() //이미지 좌우 변환
    {
        if (targetToThis.x < 0)
            spriteRenderer.flipX = true;
        else if (targetToThis.x > 0)
            spriteRenderer.flipX = false;

    }



   

    public virtual void TakeDamage(int value)
    {
        //if (hp <= 0)
        //    return;  
        hp -= value;
        //데미지 이펙트
        GameMgr.Inst.DamageTxtEffect.GetObj().SetDamageTxt(value, damageTxtPos.position);

        if (hp <= 0)
            MonsterState_Update(Monster_State.Die);
    }

  protected   IEnumerator Die_Co()
    {

        //경험치볼 생성
        GameMgr.Inst.SpawnExpBall(transform.position, 2);
        //코인 생성
        if (Random.Range(0, 3) == 0)
            GameMgr.Inst.SpawnCoin(transform.position);

        //넉백 타겟과의 반대 방향으로
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(targetToThis.normalized * -1 * 300.0f);

        float a = 1;
        Color color = Color.white;
        collider.enabled = false;

        while (a > 0) //투명해지면서 사라지는
        {
            yield return null;
            a -= Time.deltaTime;
            color.a = a;
            spriteRenderer.color = color;
        }

        Destroy(this.gameObject);
    }
}
