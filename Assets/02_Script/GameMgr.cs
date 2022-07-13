using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameMgr : MonoBehaviour
{
    static public GameMgr Inst;

    public HeroCtrl hero;

    [Header("ObjPool")]
    public DamageTxtEffect DamageTxtEffect;
    public Monsters_P monsters_P;
    public PlayerHitEffect_P playerHitEffect_P;



    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        StartCoroutine(MonsterSpawner());  
    }

    IEnumerator MonsterSpawner()
    {
        WaitForSeconds spanwTime = new WaitForSeconds(2.0f);
     
        while (true)
        {
            yield return spanwTime;
            Monster newMonster = monsters_P.GetObj();
            newMonster.SetStatus(30);
            newMonster.transform.position = Vector3.zero;

        }
    }
    


}
