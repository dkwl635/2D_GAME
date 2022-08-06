using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroEvent : MonoBehaviour
{
    //애니메이션 이벤트에 연결하기 위한 HeroEvent
    public HeroCtrl HeroCtrl;
    public void Attack_Event() //공격하는 이벤트 함수
    {
        HeroCtrl.Attack_Event();
    }
}
