using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeroCtrl : MonoBehaviour
{
    //HeroCtrl 캐릭터의 움직임 및 능력치 관련
    public Transform heroModelTr; //케릭터으 모델Transform
    public HeroModel heroModel; //장비 장착을 위한 

    private HeroCtrlMgr HeroCtrlMgr; //캐릭터와 관련된 UI
    private Animator animator; //애니메이터
    private Rigidbody2D rigidbody; //이동 물리 
    private SortingGroup sortingGroup; //캐릳터 이미지의 layerSort를 위하여

    private Vector3 mvDir = Vector3.zero; //방향백터

    [Header("Move")]
    [SerializeField] private float speed = 2; //이동속도
    
    //처음 시작시 원래 스케일저장
    private Vector3 originScale; 
    [Header("Attack")] //공격관련
    [SerializeField] private GameObject attackPoint; //공격포인트(위치확인용)
    [SerializeField] private Vector2 attackBox = new Vector2(3, 3);//공격 범위


    [Header("PlayerAbility")] //능력치
    [SerializeField] private int hp = 100;
    public int maxHp = 100;            
    public int attackPower = 10;
    public int def = 0;
    public int skillPower = 1;
    [SerializeField] private int Lv = 1;
    [SerializeField] private int curExp = 0;
    [SerializeField] private int maxExp = 10;
    [SerializeField] private float skillCool = 100.0f;

    [Header("Inven")] //인벤
    [SerializeField] int coin = 0;
    //부위별 장착된 아이템 저장 딕셔너리 변수
    Dictionary<EquipmentType, EquipmentItem> equipmentItems = new Dictionary<EquipmentType, EquipmentItem>();
    public Dictionary<EquipmentType, EquipmentItem> EquipmentItems { get { return equipmentItems; } }
    public int AddAttPw //장찯된 아이템의 데미지를 합산하여 반환
    {
        get 
        {
            int value = 0;
            if (equipmentItems.ContainsKey(EquipmentType.Weapon_R))
                value += equipmentItems[EquipmentType.Weapon_R].value;
            if (equipmentItems.ContainsKey(EquipmentType.Weapon_L))
                value += equipmentItems[EquipmentType.Weapon_L].value;

            return value;
        }
    }

    public int AddDef //장착된 아이템의 방어력를 합산하여 반환
    {
        get
        {
            int value = 0;
            if (equipmentItems.ContainsKey(EquipmentType.Shield))
                value += equipmentItems[EquipmentType.Shield].value;
            if (equipmentItems.ContainsKey(EquipmentType.Armor))
                value += equipmentItems[EquipmentType.Armor].value;
            if (equipmentItems.ContainsKey(EquipmentType.Plant))
                value += equipmentItems[EquipmentType.Plant].value;

            return value;
        }
    }

    public int Coin //가지고 있는코인 반환, 코인의 변화가 생기면 관련 UI새로고침
    {
        get { return coin; }
        set
        {
            coin = value;
            HeroCtrlMgr.SetCoin(Coin);
        }
    }

    public int Hp //체력 리턴, 체력의 변화가 생기면 채력UI 갱신
    {
        get { return hp; }
        set
        {
            hp = value;
            HeroCtrlMgr.SetHpImg(hp, (float)hp / (float)maxHp);
        }
    }

    public float SkillCool
    {
        get { return skillCool; }
        set { skillCool = value; }
    }

    public LayerMask monsterLayer; //몬스터 레이어

    //레벨업 시 사용할 이벤트 함수
    public delegate void Event();
    public event Event LevelUP_Event;


    private void Awake()
    {  //컴포넌트 등록
        animator = GetComponentInChildren<Animator>();
        HeroCtrlMgr = GetComponent<HeroCtrlMgr>();
        rigidbody = GetComponent<Rigidbody2D>();
        sortingGroup = GetComponentInChildren<SortingGroup>();
        heroModel = GetComponent<HeroModel>();
    }

    private void Start()
    {
        //기본 셋팅
        originScale = heroModelTr.localScale;
        Hp = maxHp;       
    }

    private void FixedUpdate()
    { //이동 애니메이션일때 이동하기
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            rigidbody.velocity = mvDir * speed ;
        else//이동 애니메이션이 아니면 정지
            rigidbody.velocity = Vector2.zero;
    }

    private void Update()
    {   //이미지의 순서를 위치y값을 이용하여 조절
        sortingGroup.sortingOrder = -1 * (int)transform.position.y;
    }

    public void SetJoyStickMv(Vector3 dir, bool sprint = false) //조이스틱을 이용한 이동함수
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return; //만약 공격중이면

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
            heroModelTr.localScale = originScale;
        else if(mvDir.x > 0)
        {
            Vector3 temp = originScale;
            temp.x *= -1;
            heroModelTr.localScale = temp;
        }
    }

    public void Attack() //공격
    {
        //현재 공격중이면 리턴
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;      
        //공격 트리거 적용
        animator.SetTrigger("Attack");   
    }

    public void Attack_Event()
    {//공격 애니메이션에 등록되는 함수
        GameMgr.Inst.SoundEffectPlay("AtkSound");
        //공격포인터 중심에서 네모 크기 만큼 펼쳐 충동된 콜라이더 가져오기
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.transform.position, attackBox, 0 , monsterLayer);
        
        for (int i = 0; i < hits.Length; i++)            //데미지 주기 
            hits[i].SendMessage("TakeDamage", attackPower + AddAttPw);
        
    }


    public void TakeDamage(int value)
    {
        if (hp <= 0) return; //이미 체력이 0이면
        int resultValue = value - (def + AddDef);//최종 대미지
        if (resultValue <= 0) resultValue = 1; //아무리 방어력이 높아도 1은 들어오도록
        //피격 이펙트 뿌리기
        GameMgr.Inst.playerHitEffect_P.GetObj().SetEffect(transform.position, HitType.nomarl);
        //데미지 적용
        Hp = Hp - (value - (def + AddDef));
        
       if(Hp <= 0) //사망
        {
            rigidbody.isKinematic = true; //캐릭터 고정시키기
            GameMgr.Inst.GameOver();    //게임오버
        }      
    }

    public void GetExp(int value) //경험치+
    {
        curExp += value; //현재 경험치++
        if(maxExp <= curExp)//레벨업
        {
            curExp = 0;
            maxExp = (int)(maxExp * 1.3f);//다음 경험치 목표
            LevelUp();
        }
        //경험치 UI 적용
        HeroCtrlMgr.SetExpImg(Lv, curExp == 0 ? 0 : (float)curExp / (float)maxExp );
    }

    void LevelUp() //레벨업
    {
        Lv ++;
        LevelUP_Event?.Invoke();//레벨업시 발동되는 함수 호출
    }

    public void SetEqItem(EquipmentItem item) //장비 장착
    {
        heroModel.SetEqItem(item); //모델(이미지) 적용
        
        //인벤토리에 넣기
        if (equipmentItems.ContainsKey(item.Type))
            equipmentItems[item.Type] = item;
        else
            equipmentItems.Add(item.Type, item);

    }
}
