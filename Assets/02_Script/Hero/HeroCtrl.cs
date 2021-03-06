using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeroCtrl : MonoBehaviour
{
    Transform tr;
    public Transform heroModel;
    Animator animator;
    Rigidbody2D rigidbody;
    HeroCtrlMgr HeroCtrlMgr; //UI용
    SortingGroup sortingGroup;

    Vector3 mvDir = Vector3.zero;
  
    [Header("Move")]
    [SerializeField] private float speed = 2;
  
    private Vector3 originScale;
    [Header("Attack")]
    public GameObject attackPoint;
    public int AttackPower = 10;
    public int skillPower = 1;

    [Header("PlayerStatus")]
    [SerializeField] int hp = 100;
    public int maxHp = 100;
    public int def = 0;
    public int Lv = 1;
    public int curExp = 0;
    public int maxExp = 10;
    public float skillCool = 1.0f;

    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            HeroCtrlMgr.SetHpImg(hp, (float)hp / (float)maxHp);
        }
    }
    
    public LayerMask monsterLayer;

    public delegate void Event();
    public Event LevelUP_Event;


    private void Awake()
    {
        tr = transform;       
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
            rigidbody.velocity = mvDir * speed ;
        else
            rigidbody.velocity = Vector2.zero;
    }

    private void Update()
    {
        sortingGroup.sortingOrder = -1 * (int)tr.position.y;
    }

    public void SetJoyStickMv(Vector3 dir, bool sprint = false)
    {
        mvDir = dir; //이동 방향 적용
      

        //애니메이터 적용
        if (mvDir.Equals(Vector3.zero)) animator.SetBool("move", false);
        else animator.SetBool("move", true);

        //달리기 적용
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

        //이미지 좌우 변경
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
        //현재 공격중이면 리턴
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        animator.SetTrigger("Attack");   
    }

    public void Attack_Event()
    {
        //공격포인터 중심에서 네모 크기 만큼 펼쳐 충동된 콜라이더 가져오기
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.transform.position, new Vector2(2, 2), 0 , monsterLayer);
        
        for (int i = 0; i < hits.Length; i++)            //데미지 주기 
            hits[i].SendMessage("TakeDamage", AttackPower);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint.transform.position, new Vector2(2, 2));
    }

    public void TakeDamage(int value)
    {
        if (value - def <= 0)
            return;
        
       Hp = Hp - (value - def);
          
    }

    public void GetExp(int value)
    {
        curExp += value;

        if(maxExp <= curExp)
        {
            curExp = 0;
            maxExp = (int)(maxExp * 1.5f);

            LevelUp();
        }

        HeroCtrlMgr.SetExpImg(Lv, curExp == 0 ? 0 : (float)curExp / (float)maxExp );

    }

    void LevelUp()
    {
        Lv ++;
        LevelUP_Event?.Invoke();
    }
}
