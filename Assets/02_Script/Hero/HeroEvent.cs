using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroEvent : MonoBehaviour
{
    public HeroCtrl HeroCtrl;
    public void Attack_Event()
    {
        HeroCtrl.Attack_Event();
    }
}
