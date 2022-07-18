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
            skillInfo.text = "��ų ŉ���ϱ�\n\t" + Skill.SkillInfo;
            return;
        }

        //���� ���� :
        //���� ���� :
        //���� ���� :
        string front = "";
        if (Skill.skill_Lv == 0)
            front = "��ų����";
        else if (Skill.skill_Lv == 1)
            front = "�⺻ ��ų";
        else
            front = Skill.skillLvInfo[Skill.skill_Lv - 1];

        string now = "";

        if (Skill.skill_Lv == 0)
            now = "�⺻��ų";
        else
            now = Skill.skillLvInfo[Skill.skill_Lv];

        string next = "";
        if (Skill.skill_Lv == Skill.skill_MaxLv)
            next = "�ִ� ����";
        else
            next = Skill.skillLvInfo[Skill.skill_Lv + 1];


        skillInfo.text = "���� ���� : " + front + "\n";
        skillInfo.text += "���� ���� : " + now + "\n";
        skillInfo.text += "���� ���� : " + next;

       
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
