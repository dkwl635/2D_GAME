using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCtrlMgr : MonoBehaviour
{
    //HeroCtrlMgr => UI�� HeroCtrl�� �����ϱ� ����
    JoyStick JoyStick;
    HeroCtrl HeroCtrl; //����� ������Ʈ�� ����

    public Button attackBtn;

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


}
