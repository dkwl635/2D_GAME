using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : Card
{
    //��ų ī�� �� ���� ���۽�
    //�����ϴ� ī��
    public Skill skill; //��ϵ� ��ų
    public Button skillBtn; //������ ��ų ��ư
    private void Start()
    {
        skillBtn.onClick.AddListener(GetSkill);//��ư���
    }
     
    void GetSkill()
    {
        skill.LevelUp();//��ų ������
        GameMgr.Inst.StageGameStart();//�������� ����
    }

}
