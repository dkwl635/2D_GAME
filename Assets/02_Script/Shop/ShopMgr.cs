using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class ShopMgr : MonoBehaviour
{
    public static ShopMgr Inst; //싱글톤
    
    public GameObject shopObj;//필드에 소환되는 상점
   
    [Header("ShopUI")]
    public GameObject shopCanvas; //상점 UI
    public Button closeBtn;              //나가기 버튼
    public TextMeshProUGUI coinTxt;//소지금액 UI
    public GameObject yesOrNoBox; //최종구매 확인박스
    public Button yesBtn; //구매 확인버튼
    public Button noBtn;  //구매 취소버튼

    [Header("ShopCard")]
    public ShopEqCard[] shopEqCards; //3개 //상점 장비 판매카드
    public ShopPortionCard[] shopPortionCards; //2개 //상점 포션 판매카드
    
    [Header("ShopItemList")]//종류별 판매 목록
    [SerializeField] private PortionItem[] portionItems; 
    [SerializeField] private EquipmentItem[] eqItems;

    public delegate void Event();
    public Event BuyEvent; //구매시 호출되는 함수

    EquipmentItem waitItem; //최종구매전까지 대기하는 함수

    public bool Shop //상점 오픈변수
    {
        set
        {
            shopObj.SetActive(value);
            if (value)//상점 오픈시 판매목록 셋팅
                ShopSetting();
        }
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

  
    public void ShopOpen() //상점UI On
    {
        if (shopCanvas.activeSelf)
            return;

        RefreshCoin();
        yesOrNoBox.gameObject.SetActive(false);
        shopCanvas.gameObject.SetActive(true);        
    }

    void ShopOff() //상점 UI Off
    {
        GameMgr.Inst.OffEqItemInfoBox();
        shopCanvas.gameObject.SetActive(false); 
    }

   public void ShopSetting() //판매목록 셋팅
    {
        BuyEvent = null; //등록된 함수 비우기
        BuyEvent += RefreshCoin; //구매시 호출될 함수 넣어주기
        //판매목록 랜덤값을 이용하여 채워주기
        shopPortionCards[0].SetCard(portionItems[Random.Range(0, portionItems.Length)]);
        shopPortionCards[1].SetCard(portionItems[Random.Range(0, portionItems.Length)]);

        shopEqCards[0].SetCard(eqItems[Random.Range(0, eqItems.Length)]);
        shopEqCards[1].SetCard(eqItems[Random.Range(0, eqItems.Length)]);
        shopEqCards[2].SetCard(eqItems[Random.Range(0, eqItems.Length)]);
    }

    void RefreshCoin() //소지금 새로고침
    {
        coinTxt.text = GameMgr.Inst.hero.Coin.ToString();
    }
   
    public void BuyPortion(PortionItem item) //포션 구매함수
    {
        GameMgr.Inst.SoundEffectPlay("UesCoin");//효과음
        GameMgr.Inst.hero.Coin -= item.price; //소지금에서 판매금액 빼기
        BuyEvent(); //구매시 호출된함수 
        switch (item.AbilityType) //타입별 능력치 적용
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

    public void BuyEqItem(EquipmentItem item) //장비 구매
    {
        waitItem = item; //장비는 한번 확인을 위해 잠시대기
        yesOrNoBox.gameObject.SetActive(true); //최종확인 박스
    }

    public void Ok_Btn() //구매확인버튼
    {
        GameMgr.Inst.SoundEffectPlay("UesCoin"); //효과음
        GameMgr.Inst.hero.Coin -= waitItem.price; //구매금액빼기
        BuyEvent(); //구매시 함수 호출
        //구매 확정된 아이템 장비 시켜주기
        GameMgr.Inst.hero.SetEqItem(waitItem);
        waitItem = null;
        //박스 off
        yesOrNoBox.gameObject.SetActive(false);
    }
    public void No_Btn() //구매 취소
    {
        waitItem = null; //대기중인 아이템 비우기
        yesOrNoBox.gameObject.SetActive(false);//박스 off
    }
}
