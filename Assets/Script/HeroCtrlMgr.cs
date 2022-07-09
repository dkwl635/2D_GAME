using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCtrlMgr : MonoBehaviour
{
    JoyStick JoyStick;
    HeroCtrl HeroCtrl;

    public Button attackBtn;

    private void Start()
    {
        JoyStick = GameObject.FindObjectOfType<JoyStick>();
        HeroCtrl = GameObject.FindObjectOfType<HeroCtrl>();

        JoyStick.heroCtrl = HeroCtrl;

        attackBtn.onClick.AddListener(AttackBtnFunc);
    }

    void AttackBtnFunc()
    {
        HeroCtrl.Attack();
    }


}
