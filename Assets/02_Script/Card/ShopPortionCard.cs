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

    private void Start()
    {
        buyBtn.onClick.AddListener(BuyItem);
    }

    public void SetCard(PortionItem item)
    {
        buyItme = item;
        image.sprite = item.img;

        info.text = item.itemName;
        priceTxt.text = "АЁАн : " + item.price;


        Refresh();
        ShopMgr.Inst.BuyEvent += Refresh;
    }

    void Refresh()
    {
        if (GameMgr.Inst.hero.Coin >= buyItme.price)
            buyBtn.interactable = true;
        else
            buyBtn.interactable = false;
    }

    public void BuyItem()
    {
       
        ShopMgr.Inst.BuyPortion(buyItme);
        ShopMgr.Inst.BuyEvent();
    }

}
