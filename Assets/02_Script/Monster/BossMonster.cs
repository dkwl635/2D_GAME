using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper; //데미지함수
using CardHelp;         //카드 함수

public class BossMonster : MonoBehaviour, ITakeDamage ,SetCard
{
    public enum Monster_State //보스 몬스터 상태
    {
        Idle,
        Move,
        Attack1,
        Attack2,
        Die
    }

    protected HeroCtrl targetHero; //히어로 클레스
    protected Transform targetTr; //타겟 : 플레이어 
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;            
    protected Rigidbody2D rigidbody;
    protected Collider2D collider;

    [Header("MonsterStatus")]
    [SerializeField] protected Sprite monsterImg; //카드에 사용할 몬스터 이미지
    [SerializeField] protected float speed = 2.0f; //이동 속도
    [SerializeField] protected int hp = 30;
    [SerializeField] protected int attackPower = 10;
    [SerializeField] protected float attakcDis = 4;

    [SerializeField] protected Transform damageTxtPos;
    [SerializeField] protected LayerMask heroLayer;

    protected  Vector3 targetToThis = Vector3.zero; //타겟과의 거리를 구하기 위해
    protected  Vector3 dir = Vector3.zero; // 방향
    protected float dis;                            //거리
    protected Monster_State monster_State;

 
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
        targetToThis = targetTr.position - transform.position; //타겟과의 거리관계
        dir = targetToThis.normalized;     //방향값
        dis = targetToThis.sqrMagnitude;  //거리 길이 변환
        Flip_Update();//이미지 좌우변환
    }


    protected virtual void  MonsterState_Update(Monster_State newStatue) //몬스터의 상태 변화에 따른 애니메이터 변환해주기
    {       
        if (monster_State.Equals(newStatue)) return; //이전과 같은 상태면 리턴

        if (monster_State.Equals(Monster_State.Die)) return; //죽은 상태면

        //새로운 상태 적용
        monster_State = newStatue;
        //속도 초기화
        rigidbody.velocity = Vector2.zero;
        //상태별 애니메이션 적용
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
  protected void Flip_Update() //이미지 좌우 변환
    {
        if (targetToThis.x < 0)
            spriteRenderer.flipX = true;
        else if (targetToThis.x > 0)
            spriteRenderer.flipX = false;

        //위아래 구분을 위한
        spriteRenderer.sortingOrder = -1 * (int)transform.position.y;
    }



    public virtual void TakeDamage(int value)
    {   
        hp -= value;
        //데미지 이펙트
        GameMgr.Inst.DamageTxtEffect_P.GetObj().SetDamageTxt(value, damageTxtPos.position);

        if (hp <= 0)//죽음
            MonsterState_Update(Monster_State.Die);
    }

  protected   IEnumerator Die_Co()
    {//죽었을때
        GameMgr.Inst.BossKill();
        //경험치볼 생성 (5의 경험치 5개 생성)
        for (int i = 0; i < 5; i++)
            GameMgr.Inst.SpawnExpBall(transform.position, 5);

        //코인 생성 시도 .. 15번 3분1 확률
        for (int i = 0; i < 15; i++)
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

        Destroy(this.gameObject); //오브젝트 파괴
    }

    public CardData GetCard()//카드로 표기할시
    {
        CardData card;
        card.img = monsterImg;
        card.info = "Hp : " + hp + "\nAttack : " + attackPower;
        return card;
    }
}
