using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : Card
{
    //스킬 카드 매 라운드 시작시
    //선택하는 카드
    public Skill skill; //등록된 스킬
    public Button skillBtn; //레벨업 스킬 버튼
    private void Start()
    {
        skillBtn.onClick.AddListener(GetSkill);//버튼등록
    }
     
    void GetSkill()
    {
        skill.LevelUp();//스킬 레벨업
        GameMgr.Inst.StageGameStart();//스테이지 시작
    }

}
