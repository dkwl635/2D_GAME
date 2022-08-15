using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class ShopMgr : MonoBehaviour
{
    public static ShopMgr Inst; //�̱���
    
    public GameObject shopObj;//�ʵ忡 ��ȯ�Ǵ� ����
   
    [Header("ShopUI")]
    public GameObject shopCanvas; //���� UI
    public Button closeBtn;              //������ ��ư
    public TextMeshProUGUI coinTxt;//�����ݾ� UI
    public GameObject yesOrNoBox; //�������� Ȯ�ιڽ�
    public Button yesBtn; //���� Ȯ�ι�ư
    public Button noBtn;  //���� ��ҹ�ư

    [Header("ShopCard")]
    public ShopEqCard[] shopEqCards; //3�� //���� ��� �Ǹ�ī��
    public ShopPortionCard[] shopPortionCards; //2�� //���� ���� �Ǹ�ī��
    
    [Header("ShopItemList")]//������ �Ǹ� ���
    [SerializeField] private PortionItem[] portionItems; 
    [SerializeField] private EquipmentItem[] eqItems;

    public delegate void Event();
    public Event BuyEvent; //���Ž� ȣ��Ǵ� �Լ�

    EquipmentItem waitItem; //�������������� ����ϴ� �Լ�

    public bool Shop //���� ���º���
    {
        set
        {
            shopObj.SetActive(value);
            if (value)//���� ���½� �ǸŸ�� ����
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

  
    public void ShopOpen() //����UI On
    {
        if (shopCanvas.activeSelf)
            return;

        RefreshCoin();
        yesOrNoBox.gameObject.SetActive(false);
        shopCanvas.gameObject.SetActive(true);        
    }

    void ShopOff() //���� UI Off
    {
        GameMgr.Inst.OffEqItemInfoBox();
        shopCanvas.gameObject.SetActive(false); 
    }

   public void ShopSetting() //�ǸŸ�� ����
    {
        BuyEvent = null; //��ϵ� �Լ� ����
        BuyEvent += RefreshCoin; //���Ž� ȣ��� �Լ� �־��ֱ�
        //�ǸŸ�� �������� �̿��Ͽ� ä���ֱ�
        shopPortionCards[0].SetCard(portionItems[Random.Range(0, portionItems.Length)]);
        shopPortionCards[1].SetCard(portionItems[Random.Range(0, portionItems.Length)]);

        shopEqCards[0].SetCard(eqItems[Random.Range(0, eqItems.Length)]);
        shopEqCards[1].SetCard(eqItems[Random.Range(0, eqItems.Length)]);
        shopEqCards[2].SetCard(eqItems[Random.Range(0, eqItems.Length)]);
    }

    void RefreshCoin() //������ ���ΰ�ħ
    {
        coinTxt.text = GameMgr.Inst.hero.Coin.ToString();
    }
   
    public void BuyPortion(PortionItem item) //���� �����Լ�
    {
        GameMgr.Inst.SoundEffectPlay("UesCoin");//ȿ����
        GameMgr.Inst.hero.Coin -= item.price; //�����ݿ��� �Ǹűݾ� ����
        BuyEvent(); //���Ž� ȣ����Լ� 
        switch (item.AbilityType) //Ÿ�Ժ� �ɷ�ġ ����
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

    public void BuyEqItem(EquipmentItem item) //��� ����
    {
        waitItem = item; //���� �ѹ� Ȯ���� ���� ��ô��
        yesOrNoBox.gameObject.SetActive(true); //����Ȯ�� �ڽ�
    }

    public void Ok_Btn() //����Ȯ�ι�ư
    {
        GameMgr.Inst.SoundEffectPlay("UesCoin"); //ȿ����
        GameMgr.Inst.hero.Coin -= waitItem.price; //���űݾ׻���
        BuyEvent(); //���Ž� �Լ� ȣ��
        //���� Ȯ���� ������ ��� �����ֱ�
        GameMgr.Inst.hero.SetEqItem(waitItem);
        waitItem = null;
        //�ڽ� off
        yesOrNoBox.gameObject.SetActive(false);
    }
    public void No_Btn() //���� ���
    {
        waitItem = null; //������� ������ ����
        yesOrNoBox.gameObject.SetActive(false);//�ڽ� off
    }
}
