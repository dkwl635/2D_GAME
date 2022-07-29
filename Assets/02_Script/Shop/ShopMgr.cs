using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class ShopMgr : MonoBehaviour
{
    public static ShopMgr Inst;

    public GameObject shopObj;
    public GameObject shopCanvas;
    public Button closeBtn;

    [Header("ShopCard")]
    public ShopEqCard[] shopEqCards; //2°³
    public ShopPortionCard[] shopPortionCards; //2°³
    
    [Header("ShopItemList")]
    public PortionItem[] portionItems;
    public EquipmentItem[] eqItems;

    [Header("ShopUI")]
    public TextMeshProUGUI coinTxt;
    public GameObject yesOrNoBox;
    public Button yesBtn;
    public Button noBtn;


    public delegate void Event();
    public Event BuyEvent;

    EquipmentItem waitItem;


    public bool Shop
    {
        set { shopObj.SetActive(value); }
    }

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(ShopOff);
        yesBtn.onClick.AddListener(Ok_Btn);
        noBtn.onClick.AddListener(No_Btn);
    }

   

    public void ShopOpen()
    {
        if (shopCanvas.activeSelf)
            return;
      

        yesOrNoBox.gameObject.SetActive(false);
        shopCanvas.gameObject.SetActive(true);
        BuyEvent = null;
        ShopSetting();
    }

    void ShopOff()
    {
        GameMgr.Inst.OffEqItemInfoBox();
        shopCanvas.gameObject.SetActive(false);
    }

    void ShopSetting()
    {
        RefreshCoin();
        BuyEvent += RefreshCoin;

        shopPortionCards[0].SetCard(portionItems[Random.Range(0, portionItems.Length)]);
        shopPortionCards[1].SetCard(portionItems[Random.Range(0, portionItems.Length)]);

        shopEqCards[0].SetCard(eqItems[Random.Range(0, eqItems.Length)]);
        shopEqCards[1].SetCard(eqItems[Random.Range(1, eqItems.Length)]);
        shopEqCards[2].SetCard(eqItems[Random.Range(2, eqItems.Length)]);
    }

    void RefreshCoin()
    {
        coinTxt.text = GameMgr.Inst.hero.Coin.ToString();
    }
   
    public void BuyPortion(PortionItem item)
    {
        GameMgr.Inst.hero.Coin -= item.price;
        BuyEvent();
        switch (item.AbilityType)
        {
            case AbilityType.AttackPw:
                GameMgr.Inst.hero.attackPower += item.value;
                break;
            case AbilityType.Def:
                GameMgr.Inst.hero.def += item.value;
                break;
            case AbilityType.Hp:
                GameMgr.Inst.hero.maxHp += item.value;
                GameMgr.Inst.hero.Hp += item.value;
                break;

        }   
    }

    public void BuyEqItem(EquipmentItem item)
    {     
        waitItem = item;
        yesOrNoBox.gameObject.SetActive(true);
    }

    public void Ok_Btn()
    {
        GameMgr.Inst.hero.Coin -= waitItem.price;
        BuyEvent();
        GameMgr.Inst.hero.SetEqItem(waitItem);
        waitItem = null;

        yesOrNoBox.gameObject.SetActive(false);
    }
    public void No_Btn()
    {
        waitItem = null;
        yesOrNoBox.gameObject.SetActive(false);
    }
}
