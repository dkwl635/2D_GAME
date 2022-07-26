using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroCtrlMgr : MonoBehaviour
{
    //HeroCtrlMgr => UI�� HeroCtrl�� �����ϱ� ����
    JoyStick JoyStick;
    HeroCtrl HeroCtrl; //����� ������Ʈ�� ����


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
        JoyStick = GameObject.FindObjectOfType<JoyStick>(); //JoyStick ��������
        HeroCtrl = GetComponent<HeroCtrl>(); //HeroCtrl ��������

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
