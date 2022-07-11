using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCtrlMgr : MonoBehaviour
{
    //HeroCtrlMgr => UI와 HeroCtrl를 연결하기 위한
    JoyStick JoyStick;
    HeroCtrl HeroCtrl; //히어로 오브젝트에 관한

    public Button attackBtn;

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


}
