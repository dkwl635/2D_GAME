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
    public TextMeshProUGUI coin;


    private void Start()
    {
        JoyStick = GameObject.FindObjectOfType<JoyStick>(); //JoyStick 가져오기
        HeroCtrl = GetComponent<HeroCtrl>(); //HeroCtrl 가져오기

        JoyStick.heroCtrl = HeroCtrl;

        attackBtn.onClick.AddListener(AttackBtnFunc);
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
}
