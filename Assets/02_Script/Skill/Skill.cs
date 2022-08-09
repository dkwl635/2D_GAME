using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CardHelp;
using MonsterHelper;

public abstract class Skill : MonoBehaviour , ICardLvUp
{//��ų�� ���̽��� �Ǵ� Ŭ����

    protected HeroCtrl hero; //ĳ����
    public int skill_Lv = 0; //��ų���� 
    public int skill_MaxLv = 0; //��ų �ִ� ���� �⺻������ 7�ܰ�
    public bool getSkill = false; //��ų�� ���� ��ų����

    public string[] skillLvInfo = new string[7]; //��ų ������ ��� ����
    public int[] skillPw = new int[7];               //��ų ������ ������
    public Sprite skillSprite;
   
    protected AudioSource audioSource;
    [SerializeField] protected float skillCool; //��ų ��Ÿ��   
    protected Coroutine skill_Co; //�������� ��ų �ڷ�ƾ �Լ� �����

    string CardInfo  //ī�忡 ����� ����
    {
        get
        {
            if (getSkill) //�̹� ���� ��ų�̸� �������� ����
               return skillLvInfo[skill_Lv + 1];
            else 
                return skillLvInfo[0];
        }
    }

    //���� ��ų ��Ÿ�� (��ų ��Ÿ�� * ĳ������ ��ų ��Ÿ�� ���� 100 => 1 ���� 80 => 0.8)
    public float SkillCool { get { return skillCool * (hero.SkillCool *0.01f); } }

    //���� ��ų ������ ĳ������ �߰� ��ų�������� �ջ��Ͽ� ���
    public int SkillDamage { get { return skillPw[skill_Lv] + hero.skillPower; } }

    private void Awake()
    {
        hero = FindObjectOfType<HeroCtrl>();
        audioSource =GetComponent<AudioSource>();
    }

    private void Start()
    {
        Skill_Init(); //��ų �ʱ� ������ �ϴ�
    }
    public virtual void Skill_Init(){ }//��ų �ʱ� ������ �ϴ�
    public void SkillStart() //��ų�� �����ϴ� �Լ�
    {
        if(skill_Co != null) //���� ���� ��ų�� ���ư��� ���̸�
        {//�ߴ� �� �ٽ� ����
           StopCoroutine(skill_Co);
            skill_Co = null;
        }
       //��ų �����ϱ�
       skill_Co =  StartCoroutine(SkillStart_Co());
    }

    public void TakeMonsterDamage(ITakeDamage monster)
    {//���Ϳ��� �������� �ִ� �ݶ��̴����� �� �Լ��� ȣ��
        monster.TakeDamage(SkillDamage);
    }

    public abstract IEnumerator SkillStart_Co();//��ų�� ���ư��� �ڷ�ƾ

   
    
    public void SkillLvUp() //��ų ������ �����ִ�
    {        
        skill_Lv++;
        SkillRefresh();
        SkillStart();
    }

    public abstract void SkillRefresh();//��ų ���¸� �ʱ�ȭ ���� //��ų ������ ���� ȿ��

    protected GameObject FindNearestObjectByTag(string tag)//���� ����� �±׷� �˻��ؼ� ������Ʈ�� ã����
    {
        // Ž���� ������Ʈ ����� List �� �����մϴ�.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();
        // LINQ �޼ҵ带 �̿��� ���� ����� ���� ã���ϴ�.
        var neareastObject = objects
            .OrderBy(obj =>
            {//�Ÿ��� ����Ͽ�
                return Vector3.Distance(hero.transform.position, obj.transform.position);
            })
        .FirstOrDefault(); //ù��° ��� ��ȯ ������ null;

        return neareastObject;
    }

    public CardData GetCard() //ī�嵥���� ��ȯ
    {
        CardData card;
        card.img = skillSprite;
        card.info = CardInfo;
        return card;
    }

    public bool LevelPossible() //�������� ��������
    {
        if (!getSkill || skill_Lv == skill_MaxLv)
            return false;
        else
            return true;
    }

    public void LevelUp() //ī�带 ���� ������
    {
        if (getSkill)
            SkillLvUp();
        else
        {
            getSkill = true;
            this.gameObject.SetActive(true);         
        }
    }
}
