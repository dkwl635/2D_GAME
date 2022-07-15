using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpPanel : MonoBehaviour
{
    HeroCtrl hero;

    private void Start()
    {
        hero = GameMgr.Inst.hero;
    }

    public void AtkUpBtn()
   {
        hero.AttackPower += 1;
        OffPanel();
   }

    public void DefUpBtn()
    {
        hero.def += 1;
        OffPanel();
    }

    public void SpeedUpBtn()
    {
        Debug.Log("이동속도 증가" );
        OffPanel();
    }

  public  void OffPanel()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;

    }

}
