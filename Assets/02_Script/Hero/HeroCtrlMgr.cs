using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroCtrlMgr : MonoBehaviour
{
    //HeroCtrlMgr => UI�� HeroCtrl�� �����ϱ� ����
    HeroCtrl HeroCtrl; //����� ������Ʈ�� ����

    [Header("Atk_UI")]
    public Button attackBtn; //���ݹ�ư
    [Header("HP_UI")]
    public Image hpGage; //ü�¹�
    public TextMeshProUGUI hpTxt;//ü�� �ؽ�Ʈ
    [Header("LV_UI")]
    public Image lvGage;//������
    public TextMeshProUGUI lvTxt;//���� �ؽ�Ʈ

    [Header("Inven_UI")]
    public Button infoBtn;//���� â�� ���� ��ư
    public GameObject infoBox;  //����â �ڽ�
    public TextMeshProUGUI coin;    //���ΰ��� �ؽ�Ʈ
    public TextMeshProUGUI infoTxt; //�ɷ�ġ ǥ�� �ؽ�Ʈ
    public GameObject[] EqUI;   //�κ����� ������ ������ �̹���_go
    public Image[] EqUISprite;  //�̹��� ������ 
   

    private void Start()
    {
        HeroCtrl = GetComponent<HeroCtrl>(); //HeroCtrl ��������
     

        attackBtn.onClick.AddListener(AttackBtnFunc);
        infoBtn.onClick.AddListener(OnOffInfo);
    }

    void AttackBtnFunc()
    {   
        HeroCtrl.Attack();
    }

    public void SetHpImg(int hp, float value)
    {
        hpGage.fillAmount = value;
        hpTxt.text = hp.ToString();

    }

    public void SetExpImg(int lv, float value)
    {
        lvGage.fillAmount = value;
        lvTxt.text = "Lv " + lv;
    }
   
    public void SetCoin(int coin)
    {
        this.coin.text = coin.ToString();
    }

    void OnOffInfo()
    {
        if (infoBox.activeSelf)
        {
            infoBox.gameObject.SetActive(false);
            GameMgr.Inst.OffEqItemInfoBox();
        }
        else
        {
            infoBox.gameObject.SetActive(true);
            GameMgr.Inst.OffEqItemInfoBox();
            SetInfoTxt();
            EqUISet();
        }
          

    
    }

    public void EqUISet()
    {
        for (int i = 0; i < EqUI.Length; i++) 
            EqUI[i].SetActive(false);
        

        foreach (var eq in HeroCtrl.EquipmentItems)
        {
            EquipmentType type = eq.Key;
            EquipmentItem item = eq.Value;

            if (item.Type == EquipmentType.Weapon_R)
            {
                EqUI[0].SetActive(true);
                EqUISprite[0].sprite = item.img[0];
            }
            else if (type == EquipmentType.Shield)
            {
                EqUI[1].SetActive(true);
                EqUISprite[1].sprite = item.img[0];
            }
            else if (type == EquipmentType.Plant)
            {
                EqUI[2].SetActive(true);
                EqUISprite[2].sprite = item.img[0];
                EqUISprite[3].sprite = item.img[1];
            }
            else if (type == EquipmentType.Armor)
            {
                EqUI[3].SetActive(true);
                EqUISprite[4].sprite = item.img[0];
                EqUISprite[5].sprite = item.img[1];
                EqUISprite[6].sprite = item.img[2];

            }
        }
    }

    public void SetInfoTxt()
    {
        string str = "�ɷ�ġ\n\n";

        str += "ü�� : " + HeroCtrl.maxHp; 
        str += "\n���ݷ� : " + HeroCtrl.attackPower;
        if (HeroCtrl.AddAttPw > 0)
            str += " + " + HeroCtrl.AddAttPw + "(�߰� ���ݷ�)";
        str += "\n���� : " + HeroCtrl.def;
        if (HeroCtrl.AddDef > 0)
            str += " + " + HeroCtrl.AddDef + "(�߰� ����)";
        str += "\n\n�߰� ��ų ������ : " + HeroCtrl.skillPower;
        str += "\n��ų ��Ÿ�� : " + HeroCtrl.SkillCool + "%";

        infoTxt.text = str;
    }


  
}
