using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : Card
{
    public Skill skill;
    public Button skillBtn;

    private void Start()
    {
        skillBtn.onClick.AddListener(GetSkill);
    }

    void GetSkill()
    {
        skill.LevelUp();
        GameMgr.Inst.StageGameStart();
    }

}
