using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopEqCard : Card,IPointerClickHandler
{
    EquipmentItem buyItme;//판매하는 장비아이템

    //장비별 보이는 이미지
    //기본 베이스 Card의 image변수에는 무기이미지를 등록
    public Image Sheild;
    public Image[] Armors;
    public Image[] Pant;
   
    public TextMeshProUGUI priceTxt; //가격표시
    public Button buyBtn;   //구매 버튼

    private void Start()
    {
        buyBtn.onClick.AddListener(BuyItem); //버튼등록
    }

    public void SetCard(EquipmentItem item)//장비 아이템정보를 가지고 카드셋팅
    {
        buyItme = item; //구매 아이템 등록
        //우선 모든 이미지 off 하기
        image.gameObject.SetActive(false);
        Sheild.gameObject.SetActive(false);
        for (int i = 0; i < Armors.Length; i++)
            Armors[i].gameObject.SetActive(false);

        for (int i = 0; i < Pant.Length; i++)
            Pant[i].gameObject.SetActive(false);
        //우선 모든 이미지 off 하기

        //부위별로 맟추어 이미지스프라이트 적용
        if (item.Type == EquipmentType.Weapon_L || item.Type == EquipmentType.Weapon_R)
        {
            image.gameObject.SetActive(true);
            image.sprite = item.img[0];
        }
        else if (item.Type == EquipmentType.Shield)
        {
            Sheild.gameObject.SetActive(true);
            Sheild.sprite = item.img[0];
        }
        else if (item.Type == EquipmentType.Armor)
        {
            for (int i = 0; i < Armors.Length; i++)
                Armors[i].gameObject.SetActive(true);

            Armors[0].sprite = item.img[0];
            Armors[1].sprite = item.img[1];
            Armors[2].sprite = item.img[2];
        }
        else if (item.Type == EquipmentType.Plant)
        {
            for (int i = 0; i < Pant.Length; i++)
                Pant[i].gameObject.SetActive(true);

            Pant[0].sprite = item.img[0];
            Pant[1].sprite = item.img[1];
        }
        //부위별로 맟추어 이미지스프라이트 적용
        //아이템 이름 및 가격 등록
        info.text = item.itemName;
        priceTxt.text = "가격 : " + item.price;
        
        Refresh();//구매가 가능한지 판별
        //상점매니저의 구매시 다른카드들도 새로고침을 위한
        //델리게이트 함수에 등록
        ShopMgr.Inst.BuyEvent += Refresh;
    }

    void Refresh()//구매가 가능한지 판별
    {//구매 버튼을 활성할지 안할지
        if (GameMgr.Inst.hero.Coin >= buyItme.price)
            buyBtn.interactable = true;
        else
            buyBtn.interactable = false;
    }
  
   public void BuyItem()
    { //구매 하기
        ShopMgr.Inst.BuyEqItem(buyItme);      
    }

    public void OnPointerClick(PointerEventData eventData)
    {//카드 선택시 아이템 설명 패널 On 이름과 능력치 정도
        GameMgr.Inst.ShowEqItemInfo(buyItme, transform.position);
    }
}
