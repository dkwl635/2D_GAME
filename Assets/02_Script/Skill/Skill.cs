using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public HeroCtrl hero;
    public int skill_Lv = 0;
    public int skill_MaxLv = 0;
    public bool getSkill = false;

   [HideInInspector]public string[] skillLvInfo = new string[7];

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
        StopAllCoroutines();
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
        StopAllCoroutines();
        SkillRefresh();
        skill_Lv++;

        SkillStart();
    }
    
    public virtual void SkillLvDown() 
    {
        StopAllCoroutines();
        SkillRefresh();
    
        if (skill_Lv == 0)
        {
            getSkill = false;
            return;
        }

        skill_Lv--;

        SkillStart();
    }
   
    public virtual void SkillRefresh()
    {
       
    }

  

}
