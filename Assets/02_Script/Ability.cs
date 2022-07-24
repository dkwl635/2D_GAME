using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHelp;

public enum AbilityType
{
    Hp,
    AttackPw,
    Def,
    SkillPw,
    SkillCool
}
[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Object Asset/AbilityData")]
public class Ability : ScriptableObject , ICardLvUp
{
    public AbilityType abilityType;
    public Sprite sprite;
    public string Info;

    public int lv = 0;
    public int maxLv = 5;

    public int[] addAbility;

    public bool LevelPossible()
    {
        if (lv == maxLv)
            return false;
        return true;
    }

    public void LevelUp()
    {
        GameMgr.Inst.AddAblilty(abilityType, addAbility[lv]);
        lv++;
    }

    public CardData GetCard()
    {
        CardData cardData;
        cardData.img = sprite;
        cardData.info = Info;

        return cardData;
    }
}
