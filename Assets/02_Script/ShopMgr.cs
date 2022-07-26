using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopMgr : MonoBehaviour, IPointerClickHandler
{
    public static ShopMgr Inst;
    public GameObject shopCanvas;
    public Button closeBtn;

    [Header("ShopCard")]
    public ShopPortionCard[] shopPortionCards;

    [Header("ShopItemList")]
    public PortionItem[] portionItems;

    [Header("ShopUI")]
    public TextMeshProUGUI coinTxt;

    public delegate void Event();
    public Event BuyEvent;


    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(() => { shopCanvas.gameObject.SetActive(false); });
    
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ShopOpen");

        shopCanvas.gameObject.SetActive(true);
        BuyEvent = null;
        ShopSetting();
    }

    void ShopSetting()
    {
        RefreshCoin();

        shopPortionCards[0].SetCard(portionItems[0]);
    }

    void RefreshCoin()
    {
        coinTxt.text = GameMgr.Inst.hero.Coin.ToString();
    }
   


}
