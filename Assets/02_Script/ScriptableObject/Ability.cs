using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHelp;

public enum AbilityType //�ɷ�ġ ����
{
    Hp,
    AttackPw,
    Def,
    SkillPw,
    SkillCool
}
[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Object Asset/AbilityData")]
public class Ability : ScriptableObject , ICardLvUp
{//�ɷ�ġ ī�忡 ���� 
    public AbilityType abilityType;
    public Sprite sprite; //ī�忡 ���� �̹���
    public string Info; //�ɷ�ġ ����

    public int lv = 0;  //���ġ ����
    public int maxLv = 5;   

    public int addAbility; //���ġ

    int saveLv = 0; //�ʱ�ȭ�� ����

    private void OnEnable()
    {//�� ���ӽ��۽� �ʱ�ȭ ����
        lv = saveLv;
    }

    public bool LevelPossible() //�������� ��������
    {
        if (lv == maxLv)
            return false;
        return true;
    }

    public void LevelUp() //������
    {
        GameMgr.Inst.AddAblilty(abilityType, addAbility);//�ɷ� ���ġ ��ŭ ��½����ֱ�
        lv++;
    }

    public CardData GetCard() //ī��� �����ֱ� ����
    {
        CardData cardData;
        cardData.img = sprite;
        cardData.info = Info;

        return cardData;
    }
}
