using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPortionCard : Card
{
    PortionItem buyItme;

    public TextMeshProUGUI priceTxt;
    public Button buyBtn;
    public void SetCard(PortionItem item)
    {
        buyItme = item;
        image.sprite = item.img;

        info.text = item.itemName;
        priceTxt.text = "АЁАн : " + item.price;


        if (GameMgr.Inst.hero.Coin >= buyItme.price)
            buyBtn.interactable = true;


        ShopMgr.Inst.BuyEvent += Refresh;
    }

    void Refresh()
    {
        if (GameMgr.Inst.hero.Coin >= buyItme.price)
            buyBtn.interactable = true;
        else
            buyBtn.interactable = false;
    }
  

}
