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

    public void SetHpImg(int hp, int maxHp)
    {
        hpGage.fillAmount = (float)hp / (float)maxHp;
        hpTxt.text = hp.ToString();

    }

}
