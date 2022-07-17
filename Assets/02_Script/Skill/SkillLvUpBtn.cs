using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillLvUpBtn : MonoBehaviour
{
   public  Button levelUpBtn;
   public TextMeshProUGUI skillInfo;
    public Image skillImg;


    Skill curSkill = null;
    public delegate void FunctionPointer();
    FunctionPointer btnFunction;

    public void SetBtn(Skill skill, FunctionPointer function)
    {
        curSkill = skill;
        btnFunction = function;

        skillInfo.text = curSkill.SkillInfo;
        skillImg.sprite = curSkill.skillSprite;

    }

    private void Start()
    {
        levelUpBtn.onClick.AddListener(BtnFunc);
    }
    

    void BtnFunc()
    {
        btnFunction();
        curSkill.SkillLvUp();       
    }
        
}
