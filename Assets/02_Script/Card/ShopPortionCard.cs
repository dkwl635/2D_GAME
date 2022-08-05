using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPortionCard : Card
{
    PortionItem buyItme; //상점 포션 아이템 정보

    //베이스 Card info에는 이름
    public TextMeshProUGUI priceTxt; //가격
    public Button buyBtn;   //구매 버튼

    private void Start()
    {//버튼 등록
        buyBtn.onClick.AddListener(BuyItem);
    }

    public void SetCard(PortionItem item)
    {//아이템 등록
        buyItme = item;
        image.sprite = item.img;

        info.text = item.itemName;
        priceTxt.text = "가격 : " + item.price;

        Refresh();
        //상점매니저의 구매시 다른카드들도 새로고침을 위한
        //델리게이트 함수에 등록
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
    }

}
