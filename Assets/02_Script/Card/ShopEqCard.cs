using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopEqCard : Card,IPointerClickHandler
{
    EquipmentItem buyItme;//�Ǹ��ϴ� ��������

    //��� ���̴� �̹���
    //�⺻ ���̽� Card�� image�������� �����̹����� ���
    public Image Sheild;
    public Image[] Armors;
    public Image[] Pant;
   
    public TextMeshProUGUI priceTxt; //����ǥ��
    public Button buyBtn;   //���� ��ư

    private void Start()
    {
        buyBtn.onClick.AddListener(BuyItem); //��ư���
    }

    public void SetCard(EquipmentItem item)//��� ������������ ������ ī�����
    {
        buyItme = item; //���� ������ ���
        //�켱 ��� �̹��� off �ϱ�
        image.gameObject.SetActive(false);
        Sheild.gameObject.SetActive(false);
        for (int i = 0; i < Armors.Length; i++)
            Armors[i].gameObject.SetActive(false);

        for (int i = 0; i < Pant.Length; i++)
            Pant[i].gameObject.SetActive(false);
        //�켱 ��� �̹��� off �ϱ�

        //�������� ���߾� �̹�����������Ʈ ����
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
        //�������� ���߾� �̹�����������Ʈ ����
        //������ �̸� �� ���� ���
        info.text = item.itemName;
        priceTxt.text = "���� : " + item.price;
        
        Refresh();//���Ű� �������� �Ǻ�
        //�����Ŵ����� ���Ž� �ٸ�ī��鵵 ���ΰ�ħ�� ����
        //��������Ʈ �Լ��� ���
        ShopMgr.Inst.BuyEvent += Refresh;
    }

    void Refresh()//���Ű� �������� �Ǻ�
    {//���� ��ư�� Ȱ������ ������
        if (GameMgr.Inst.hero.Coin >= buyItme.price)
            buyBtn.interactable = true;
        else
            buyBtn.interactable = false;
    }
  
   public void BuyItem()
    { //���� �ϱ�
        ShopMgr.Inst.BuyEqItem(buyItme);      
    }

    public void OnPointerClick(PointerEventData eventData)
    {//ī�� ���ý� ������ ���� �г� On �̸��� �ɷ�ġ ����
        GameMgr.Inst.ShowEqItemInfo(buyItme, transform.position);
    }
}
