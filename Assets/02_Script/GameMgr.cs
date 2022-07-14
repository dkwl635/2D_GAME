using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameMgr : MonoBehaviour
{
    static public GameMgr Inst;

    public HeroCtrl hero;
    Transform heroTr;

    [Header("ObjPool")]
    public DamageTxtEffect DamageTxtEffect;
    public Monsters_P monsters_P;
    public PlayerHitEffect_P playerHitEffect_P;

    [Header("UI")]
    public TextMeshProUGUI monsterKillCountTxt;
    public GameObject startTxtObj;

    //MonsterSpawn
    //°è»ê¿ë
    int monsterkillCount;
    int maxMonsterCount;
    float x;
    float y;


    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        heroTr = hero.transform;

        startTxtObj.SetActive(true);
        StartCoroutine(MonsterSpawner());  
    }

    IEnumerator MonsterSpawner()
    {
        WaitForSeconds spanwTime = new WaitForSeconds(2.0f);
        maxMonsterCount = 20;
        monsterKillCountTxt.text = "0 / " + maxMonsterCount;

        int monsterSpawnCount = 0;
        while (maxMonsterCount > monsterSpawnCount)
        {
            yield return spanwTime;
            Monster newMonster = monsters_P.GetObj();
            newMonster.SetStatus(30);
            newMonster.transform.position = RandomSpanw();

            monsterSpawnCount++;

        }
    }

    public Vector3 RandomSpanw()
    {
        Vector3 spawnpos = Vector3.zero;

        Vector3  camWMin = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 camWMax = Camera.main.ViewportToWorldPoint(Vector3.one);

        int dir = Random.Range(0, 4);

        if(dir == 0)
        {
            y = camWMax.y + 2.0f;
            x = Random.Range(camWMin.x, camWMax.x);
        }
        else if(dir == 1)
        {
            y = camWMin.y - 2.0f;
            x = Random.Range(camWMin.x, camWMax.x);
        }
        else if(dir == 2)
        {
            x = camWMax.x + 2.0f;
            y = Random.Range(camWMin.y, camWMax.y);
        }
        else if (dir == 3)
        {
            x = camWMin.x-+ 2.0f;
            y = Random.Range(camWMin.y, camWMax.y);
        }

        spawnpos.y = y;
        spawnpos.x = x;


        return spawnpos;
    }

    public void MonsterKill()
    {
        monsterkillCount++;
        monsterKillCountTxt.text = monsterkillCount +" / " + maxMonsterCount;
    }
}
