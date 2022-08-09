using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHelp;

public enum AbilityType //능력치 종류
{
    Hp,
    AttackPw,
    Def,
    SkillPw,
    SkillCool
}
[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Object Asset/AbilityData")]
public class Ability : ScriptableObject , ICardLvUp
{//능력치 카드에 쓰일 
    public AbilityType abilityType;
    public Sprite sprite; //카드에 쓰일 이미지
    public string Info; //능력치 정보

    public int lv = 0;  //상승치 레벨
    public int maxLv = 5;   

    public int addAbility; //상승치

    int saveLv = 0; //초기화를 위한

    private void OnEnable()
    {//매 게임시작시 초기화 가능
        lv = saveLv;
    }

    public bool LevelPossible() //레벨업이 가능한지
    {
        if (lv == maxLv)
            return false;
        return true;
    }

    public void LevelUp() //레벨업
    {
        GameMgr.Inst.AddAblilty(abilityType, addAbility);//능력 상승치 만큼 상승시켜주기
        lv++;
    }

    public CardData GetCard() //카드로 보여주기 위한
    {
        CardData cardData;
        cardData.img = sprite;
        cardData.info = Info;

        return cardData;
    }
}
