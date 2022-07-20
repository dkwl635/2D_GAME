using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public HeroCtrl hero;
    public int skill_Lv = 0;
    public int skill_MaxLv = 0;
    public bool getSkill = false;

   public string[] skillLvInfo = new string[7];

    public int[] skillPw = { 1, 2, 2, 4, 4, 6, 6 };
    public string SkillInfo
    {
        get { return skillLvInfo[skill_Lv]; }
    }

    public Sprite skillSprite;

   
    private void Awake()
    {
        hero = FindObjectOfType<HeroCtrl>();  
    }

    private void Start()
    {
        Skill_Init();
    }

    
    public virtual void Skill_Init(){}
    
  
    public void SkillStart()
    {
        StartCoroutine(SkillStart_Co());
    }

    public int SkillDamage()
    {
        return skillPw[skill_Lv] + hero.skillPower;
    }

    public void TakeMonsterDamage(Monster monster)
    {
        monster.TakeDamage(SkillDamage());
    }

    public virtual IEnumerator SkillStart_Co()
    {
        yield return null;
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

}
