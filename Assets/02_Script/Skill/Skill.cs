using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CardHelp;
using MonsterHelper;

public class Skill : MonoBehaviour , ICardLvUp
{
    public HeroCtrl hero;
    public int skill_Lv = 0;
    public int skill_MaxLv = 0;
    public bool getSkill = false;

    public string[] skillLvInfo = new string[7];

    public int[] skillPw = { 1, 2, 2, 4, 4, 6, 6 };

    public AudioSource audioSource;
    
    public string SkillInfo
    {
        get { return skillLvInfo[skill_Lv]; }
    }

    string CardInfo 
    {
        get
        {
            if (getSkill)
               return skillLvInfo[skill_Lv + 1];
            else
                return skillLvInfo[0];
        }
    }

    public float skillCool;
    public float SkillCool { get { return skillCool * (hero.SkillCool *0.01f); } }
   
    public Sprite skillSprite;

    public Coroutine skill_Co;

    private void Awake()
    {
        hero = FindObjectOfType<HeroCtrl>();
        audioSource =GetComponent<AudioSource>();
    }

    private void Start()
    {
        Skill_Init();
    }

    
    public virtual void Skill_Init(){}
    
  
    public void SkillStart()
    {
        if(skill_Co != null)
        {
           StopCoroutine(skill_Co);
            skill_Co = null;
        }
           
       skill_Co =  StartCoroutine(SkillStart_Co());
    }

    public int SkillDamage()
    {
        return skillPw[skill_Lv] + hero.skillPower;
    }

    public void TakeMonsterDamage(ITakeDamage damage)
    {
        damage.TakeDamage(SkillDamage());
    }

    public virtual IEnumerator SkillStart_Co()
    {
        yield return null;
    }

    public virtual void StopSkill()
    {
       
    }

    public virtual void SkillLvUp()
    {        
        skill_Lv++;
        SkillRefresh();
        SkillStart();
    }
    
    public virtual void SkillLvDown() 
    {
        if (skill_Lv == 0)
        {
            getSkill = false;
            SkillRefresh();
        }
        else
        {
            skill_Lv--;
            SkillRefresh();
            SkillStart();
        }          
    }

    public virtual void SkillRefresh() { }

    protected GameObject FindNearestObjectByTag(string tag)//가장 가까운 유닛찾기
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = objects
            .OrderBy(obj =>
            {
                return Vector3.Distance(hero.transform.position, obj.transform.position);
            })
        .FirstOrDefault(); //첫번째 요소 반환 없으면 null;

        return neareastObject;
    }

    public CardData GetCard()
    {
        CardData card;
        card.img = skillSprite;
        card.info = CardInfo;
        return card;
    }

    public bool LevelPossible()
    {
        if (!getSkill || skill_Lv == skill_MaxLv)
            return false;
        else
            return true;
    }

    public void LevelUp()
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
