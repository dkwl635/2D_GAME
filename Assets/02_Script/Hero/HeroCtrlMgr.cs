using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroCtrlMgr : MonoBehaviour
{
    //HeroCtrlMgr => UI와 HeroCtrl를 연결하기 위한
    HeroCtrl HeroCtrl; //히어로 오브젝트에 관한

    [Header("Atk_UI")]
    public Button attackBtn; //공격버튼
    [Header("HP_UI")]
    public Image hpGage; //체력바
    public TextMeshProUGUI hpTxt;//체력 텍스트
    [Header("LV_UI")]
    public Image lvGage;//레벨비
    public TextMeshProUGUI lvTxt;//레벨 텍스트

    [Header("Inven_UI")]
    public Button infoBtn;//정보 창을 여는 버튼
    public GameObject infoBox;  //정보창 박스
    public TextMeshProUGUI coin;    //코인갯수 텍스트
    public TextMeshProUGUI infoTxt; //능력치 표시 텍스트
    public GameObject[] EqUI;   //인벤에서 장착된 아이템 이미지_go
    public Image[] EqUISprite;  //이미지 파츠별 
   

    private void Start()
    {
        HeroCtrl = GetComponent<HeroCtrl>(); //HeroCtrl 가져오기
     

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
        string str = "능력치\n\n";

        str += "체력 : " + HeroCtrl.maxHp; 
        str += "\n공격력 : " + HeroCtrl.attackPower;
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
