using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CardHelp;

public class LvUpCard : Card, IPointerClickHandler
{

    public ICardLvUp cardFunc;

    public void SetCard(ICardLvUp cardFunc)
    {
        this.cardFunc = cardFunc;

        CardData  cardData = cardFunc.GetCard();

        image.sprite = cardData.img;
        info.text = cardData.info;

        this.gameObject.SetActive(true);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        cardFunc.LevelUp();
        LevelUpPanel.inst.OffPanel();
    }

   

}
