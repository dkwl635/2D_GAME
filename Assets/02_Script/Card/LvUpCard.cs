using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CardHelp;

public class LvUpCard : Card, IPointerClickHandler
{
    public ICardLvUp cardFunc; //ī�� �������� �ʿ��� �������̽�

    public void SetCard(ICardLvUp cardFunc) //ī�带 �����ϴ� �Լ�
    {//�������̵��� �����ν� �߰��ʿ��� ������ ������
        this.cardFunc = cardFunc;
        //�⺻ ī�� ����
        base.SetCard(cardFunc.GetCard());
        //ī�� ����(�������� ������ ���¸�)
        if (cardFunc.LevelPossible())
            this.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {//ī�带 �������� �����ϴ�
        cardFunc.LevelUp(); //������
        LevelUpPanel.inst.OffPanel(); //������ �г� off
    }

}

