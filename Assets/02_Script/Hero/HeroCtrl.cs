using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeroCtrl : MonoBehaviour
{
    //HeroCtrl ĳ������ ������ �� �ɷ�ġ ����
    public Transform heroModelTr; //�ɸ����� ��Transform
    public HeroModel heroModel; //��� ������ ���� 

    private HeroCtrlMgr HeroCtrlMgr; //ĳ���Ϳ� ���õ� UI
    private Animator animator; //�ִϸ�����
    private Rigidbody2D rigidbody; //�̵� ���� 
    private SortingGroup sortingGroup; //ĳ���� �̹����� layerSort�� ���Ͽ�

    private Vector3 mvDir = Vector3.zero; //�������

    [Header("Move")]
    [SerializeField] private float speed = 2; //�̵��ӵ�
    
    //ó�� ���۽� ���� ����������
    private Vector3 originScale; 
    [Header("Attack")] //���ݰ���
    [SerializeField] private GameObject attackPoint; //��������Ʈ(��ġȮ�ο�)
    [SerializeField] private Vector2 attackBox = new Vector2(3, 3);//���� ����


    [Header("PlayerAbility")] //�ɷ�ġ
    [SerializeField] private int hp = 100;
    public int maxHp = 100;            
    public int attackPower = 10;
    public int def = 0;
    public int skillPower = 1;
    [SerializeField] private int Lv = 1;
    [SerializeField] private int curExp = 0;
    [SerializeField] private int maxExp = 10;
    [SerializeField] private float skillCool = 100.0f;

    [Header("Inven")] //�κ�
    [SerializeField] int coin = 0;
    //������ ������ ������ ���� ��ųʸ� ����
    Dictionary<EquipmentType, EquipmentItem> equipmentItems = new Dictionary<EquipmentType, EquipmentItem>();
    public Dictionary<EquipmentType, EquipmentItem> EquipmentItems { get { return equipmentItems; } }
    public int AddAttPw //��F�� �������� �������� �ջ��Ͽ� ��ȯ
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

    public int AddDef //������ �������� ���¸� �ջ��Ͽ� ��ȯ
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

    public int Coin //������ �ִ����� ��ȯ, ������ ��ȭ�� ����� ���� UI���ΰ�ħ
    {
        get { return coin; }
        set
        {
            coin = value;
            HeroCtrlMgr.SetCoin(Coin);
        }
    }

    public int Hp //ü�� ����, ü���� ��ȭ�� ����� ä��UI ����
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

    public LayerMask monsterLayer; //���� ���̾�

    //������ �� ����� �̺�Ʈ �Լ�
    public delegate void Event();
    public event Event LevelUP_Event;


    private void Awake()
    {  //������Ʈ ���
        animator = GetComponentInChildren<Animator>();
        HeroCtrlMgr = GetComponent<HeroCtrlMgr>();
        rigidbody = GetComponent<Rigidbody2D>();
        sortingGroup = GetComponentInChildren<SortingGroup>();
        heroModel = GetComponent<HeroModel>();
    }

    private void Start()
    {
        //�⺻ ����
        originScale = heroModelTr.localScale;
        Hp = maxHp;       
    }

    private void FixedUpdate()
    { //�̵� �ִϸ��̼��϶� �̵��ϱ�
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            rigidbody.velocity = mvDir * speed ;
        else//�̵� �ִϸ��̼��� �ƴϸ� ����
            rigidbody.velocity = Vector2.zero;
    }

    private void Update()
    {   //�̹����� ������ ��ġy���� �̿��Ͽ� ����
        sortingGroup.sortingOrder = -1 * (int)transform.position.y;
    }

    public void SetJoyStickMv(Vector3 dir, bool sprint = false) //���̽�ƽ�� �̿��� �̵��Լ�
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return; //���� �������̸�

        mvDir = dir; //�̵� ���� ����
        //�ִϸ����� ����
        if (mvDir.Equals(Vector3.zero)) animator.SetBool("move", false);
        else animator.SetBool("move", true);
        //�޸��� ����
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
              
        //�̹��� �¿� ����
        if (mvDir.x < 0)
            heroModelTr.localScale = originScale;
        else if(mvDir.x > 0)
        {
            Vector3 temp = originScale;
            temp.x *= -1;
            heroModelTr.localScale = temp;
        }
    }

    public void Attack() //����
    {
        //���� �������̸� ����
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;      
        //���� Ʈ���� ����
        animator.SetTrigger("Attack");   
    }

    public void Attack_Event()
    {//���� �ִϸ��̼ǿ� ��ϵǴ� �Լ�
        GameMgr.Inst.SoundEffectPlay("AtkSound");
        //���������� �߽ɿ��� �׸� ũ�� ��ŭ ���� �浿�� �ݶ��̴� ��������
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.transform.position, attackBox, 0 , monsterLayer);
        
        for (int i = 0; i < hits.Length; i++)            //������ �ֱ� 
            hits[i].SendMessage("TakeDamage", attackPower + AddAttPw);
        
    }


    public void TakeDamage(int value)
    {
        if (hp <= 0) return; //�̹� ü���� 0�̸�
        int resultValue = value - (def + AddDef);//���� �����
        if (resultValue <= 0) resultValue = 1; //�ƹ��� ������ ���Ƶ� 1�� ��������
        //�ǰ� ����Ʈ �Ѹ���
        GameMgr.Inst.playerHitEffect_P.GetObj().SetEffect(transform.position, HitType.nomarl);
        //������ ����
        Hp = Hp - (value - (def + AddDef));
        
       if(Hp <= 0) //���
        {
            rigidbody.isKinematic = true; //ĳ���� ������Ű��
            GameMgr.Inst.GameOver();    //���ӿ���
        }      
    }

    public void GetExp(int value) //����ġ+
    {
        curExp += value; //���� ����ġ++
        if(maxExp <= curExp)//������
        {
            curExp = 0;
            maxExp = (int)(maxExp * 1.3f);//���� ����ġ ��ǥ
            LevelUp();
        }
        //����ġ UI ����
        HeroCtrlMgr.SetExpImg(Lv, curExp == 0 ? 0 : (float)curExp / (float)maxExp );
    }

    void LevelUp() //������
    {
        Lv ++;
        LevelUP_Event?.Invoke();//�������� �ߵ��Ǵ� �Լ� ȣ��
    }

    public void SetEqItem(EquipmentItem item) //��� ����
    {
        heroModel.SetEqItem(item); //��(�̹���) ����
        
        //�κ��丮�� �ֱ�
        if (equipmentItems.ContainsKey(item.Type))
            equipmentItems[item.Type] = item;
        else
            equipmentItems.Add(item.Type, item);

    }
}
