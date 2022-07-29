using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroCtrlMgr : MonoBehaviour
{
    //HeroCtrlMgr => UI와 HeroCtrl를 연결하기 위한
    JoyStick JoyStick;
    HeroCtrl HeroCtrl; //히어로 오브젝트에 관한


    [Header("UI")]
    public Button attackBtn;
    [Header("HP_UI")]
    public Image hpGage;
    public TextMeshProUGUI hpTxt;
    [Header("LV_UI")]
    public Image lvGage;
    public TextMeshProUGUI lvTxt;

    [Header("Inven")]
    public Button infoBtn;
    public GameObject infoBox;
    public TextMeshProUGUI coin;
    public TextMeshProUGUI infoTxt;
    public GameObject[] EqUI;
    public Image[] EqUISprite;
   

    private void Start()
    {
        JoyStick = GameObject.FindObjectOfType<JoyStick>(true); //JoyStick 가져오기
        HeroCtrl = GetComponent<HeroCtrl>(); //HeroCtrl 가져오기

        JoyStick.heroCtrl = HeroCtrl;

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
        

        foreach (var eq in HeroCtrl.equipmentItems)
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
        string str = "능력치\n\n";

        str += "체력 : " + HeroCtrl.maxHp; 
        str += "\n공격력 : " + HeroCtrl.AttackPower;
        if (HeroCtrl.AddAttPw > 0)
            str += " + " + HeroCtrl.AddAttPw + "(추가 공격력)";
        str += "\n방어력 : " + HeroCtrl.def;
        if (HeroCtrl.AddDef > 0)
            str += " + " + HeroCtrl.AddDef + "(추가 방어력)";
        str += "\n\n추가 스킬 데미지 : " + HeroCtrl.skillPower;
        str += "\n스킬 쿨타임 : " + HeroCtrl.SkillCool + "%";

        infoTxt.text = str;
    }


  
}
