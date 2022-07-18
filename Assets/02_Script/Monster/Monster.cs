using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    HeroCtrl targetHero;
    Transform targetTr; //타겟 : 플레이어 
    public SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidbody;  //HIT 시 넉백을 구현하고자 넣어보았다.
    Collider2D collider;

    enum Monster_State //몬스터 상태
    {
        Idle,
        Move,
        Attack,
        Die
    }

    [SerializeField] Monster_State monster_State = Monster_State.Idle;
    [SerializeField] float attakcDis = 2.0f;    //공격 사거리

    [Header("MonsterStatus")]
    public float speed = 2.0f; //이동 속도
    public int hp = 30;
    public int attackPower = 10;
    public Vector2 attackSize = Vector2.zero;
    public Transform damageTxtPos;




    public LayerMask heroLayer;
    Vector3 targetToThis = Vector3.zero; //타겟과의 거리를 구하기 위해
    Vector3 dir = Vector3.zero; // 방향

    private void Awake()
    {
        //초기화
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
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) //이동 애니메이션일때만 이동가능하게 하기 위해
            {
                rigidbody.velocity = Vector2.zero; //속도값은 한번 초기화
                rigidbody.velocity = dir * speed; //이동    
            }
   
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
            MonsterState_Update(Monster_State.Attack);

        Flip_Update();//이미지 좌우변환


        spriteRenderer.sortingOrder = -1 * (int)transform.position.y;

      


    }

    public void SetStatus(int hp)
    {
        this.hp = hp;
        monster_State = Monster_State.Idle;     
        gameObject.SetActive(true);
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

    void Flip_Update() //이미지 좌우 변환
    {
        if (targetToThis.x < 0)
            spriteRenderer.flipX = true;
        else if (targetToThis.x > 0)
            spriteRenderer.flipX = false;        

    }

  public  void TakeDamage(int value = 10) //데미지를 받는 함수
    {
        if (hp <= 0)
            return;

        hp -= value;
        rigidbody.velocity = Vector2.zero;
        //데미지 이펙트
        GameMgr.Inst.DamageTxtEffect.GetObj().SetDamageTxt(value, damageTxtPos.position);

        //공격중이고 죽을체력이아니면
        if (monster_State.Equals(Monster_State.Attack) && hp > 0)
            return;

        //애니메이션 실행 ... 이미 애니메이션 실행중이면
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))             
            animator.SetTrigger("Hit"); 
        
          if (hp <= 0)    
            MonsterState_Update(Monster_State.Die);
        

    }
    IEnumerator Die_Co()
    {
        //게임메니저 
        GameMgr.Inst.MonsterKill();
        //경험치볼 생성
        GameMgr.Inst.SpawnExpBall(transform.position, 2);

        //넉백 타겟과의 반대 방향으로
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(targetToThis.normalized * -1 * 3000.0f);

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

        Die_Event();
    }

    //DIE 이벤트
    public void Die_Event()
    {

        gameObject.SetActive(false);
        spriteRenderer.color = Color.white;
        GameMgr.Inst.monsters_P.ReturnObj(this);
    }

    public void Attack_Event()
    { 
        //공격포인터 중심에서 네모 크기 만큼 펼쳐 충동된 콜라이더 가져오기
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
