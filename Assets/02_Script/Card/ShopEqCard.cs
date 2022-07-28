using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopEqCard : Card,IPointerClickHandler
{
    EquipmentItem buyItme;

    public Image Sheild;
    public Image[] Armors;
    public Image[] Pant;

    public TextMeshProUGUI priceTxt;
    public Button buyBtn;

    private void Start()
    {
        buyBtn.onClick.AddListener(BuyItem);
    }

    public void SetCard(EquipmentItem item)
    {
        buyItme = item;

        image.gameObject.SetActive(false);
        Sheild.gameObject.SetActive(false);
        for (int i = 0; i < Armors.Length; i++)
            Armors[i].gameObject.SetActive(false);

        for (int i = 0; i < Pant.Length; i++)
            Pant[i].gameObject.SetActive(false);


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
            if (item.img[1] != null)
                Armors[1].sprite = item.img[1];
            else
                Armors[1].sprite = null;

            if (item.img[2] != null)
                Armors[2].sprite = item.img[2];
            else
                Armors[2].sprite = null;
        }
        else if (item.Type == EquipmentType.Plant)
        {
            for (int i = 0; i < Pant.Length; i++)
                Pant[i].gameObject.SetActive(true);

            Pant[0].sprite = item.img[0];
            Pant[1].sprite = item.img[1];
        }


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
        ShopMgr.Inst.BuyEqItem(buyItme);
        ShopMgr.Inst.BuyEvent();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameMgr.Inst.ShowEqItemInfo(buyItme, transform.position);
    }
}
