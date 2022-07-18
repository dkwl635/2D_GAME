using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSkillBtn : MonoBehaviour
{
    Skill Skill;

    public Button selSkillBtn;
    public Button skillUpBtn;
    public Button skillDownBtn;

    public Image skillImg;
    public Text skillInfo;

    private void Start()
    {
        selSkillBtn.onClick.AddListener(GetSkill);

        skillUpBtn.onClick.AddListener(SkillUp);
        skillDownBtn.onClick.AddListener(SkillDown);
       

    }

    public void SetSkill(Skill skill)
    {
        Skill = skill;

        skillImg.sprite = skill.skillSprite;

        SetText();
    }

    void GetSkill()
    {
        Skill.getSkill = true;
        SetText();
        Skill.SkillStart();
    }

    void SkillUp()
    {
        Skill.SkillLvUp();
        SetText();
    }

    void SkillDown()
    {
        Skill.SKillLvDown();
        SetText();
    }
    void SetText()
    {

        ShowBtn();
        if (!Skill.getSkill)
        {
            skillInfo.text = "스킬 흭득하기\n\t" + Skill.SkillInfo;
            return;
        }

        //이전 레벨 :
        //현제 레벨 :
        //다음 레벨 :
        string front = "";
        if (Skill.skill_Lv == 0)
            front = "스킬제거";
        else if (Skill.skill_Lv == 1)
            front = "기본 스킬";
        else
            front = Skill.skillLvInfo[Skill.skill_Lv - 1];

        string now = "";

        if (Skill.skill_Lv == 0)
            now = "기본스킬";
        else
            now = Skill.skillLvInfo[Skill.skill_Lv];

        string next = "";
        if (Skill.skill_Lv == Skill.skill_MaxLv)
            next = "최대 레벨";
        else
            next = Skill.skillLvInfo[Skill.skill_Lv + 1];


        skillInfo.text = "이전 레벨 : " + front + "\n";
        skillInfo.text += "현재 레벨 : " + now + "\n";
        skillInfo.text += "다음 레벨 : " + next;

       
    }

    void ShowBtn()
    {
        if(!Skill.getSkill)
        {
            skillUpBtn.gameObject.SetActive(false);
            skillDownBtn.gameObject.SetActive(false);         
        }
        else if(Skill.skill_Lv  == Skill.skill_MaxLv)
        {
            skillUpBtn.gameObject.SetActive(false);          
        }
        else
        {
            skillUpBtn.gameObject.SetActive(true);
            skillDownBtn.gameObject.SetActive(true);         
        }

    }
}
