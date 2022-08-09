using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CardHelp;

public class LvUpCard : Card, IPointerClickHandler
{
    public ICardLvUp cardFunc; //카드 레벨업시 필요한 인터페이스

    public void SetCard(ICardLvUp cardFunc) //카드를 셋팅하는 함수
    {//오버라이딩을 함으로써 추가필요한 정보를 가져옴
        this.cardFunc = cardFunc;
        //기본 카드 셋팅
        base.SetCard(cardFunc.GetCard());
        //카드 오픈(레벨업이 가능한 상태면)
        if (cardFunc.LevelPossible())
            this.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {//카드를 눌렀을때 반응하는
        cardFunc.LevelUp(); //레벨업
        LevelUpPanel.inst.OffPanel(); //레벨업 패널 off
    }

}

