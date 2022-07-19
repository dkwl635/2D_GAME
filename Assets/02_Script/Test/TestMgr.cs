using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMgr : MonoBehaviour
{
    GameMgr GameMgr;

    public GameObject testCanavas;

    public Button monsterSpawn_1;
    public Button monsterSpawn_2;

    public GameObject monsterSpawnPos;

    public Skill[] skills;
    public TestSkillBtn TestSkillBtn1;
    public TestSkillBtn TestSkillBtn2;

   

    private void Start()
    {
        GameMgr = GameMgr.Inst;

        monsterSpawn_1.onClick.AddListener(SpawnMonster_1);
        monsterSpawn_2.onClick.AddListener(SpawnMonster_2);


        TestSkillBtn1.SetSkill(skills[0]);
        TestSkillBtn2.SetSkill(skills[1]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            OnOffTestCanavs();
    }

    public void SpawnMonster_1()    //허수아비 스폰
    {
        Monster newMonster = GameMgr.monsters_P.GetObj();
        newMonster.SetStatus(GameMgr.monsterDatas[3]);
        newMonster.Test = true;
        newMonster.transform.position = monsterSpawnPos.transform.position;
    }

    public void SpawnMonster_2()    //따라오는몬스터 스폰
    {
        Monster newMonster = GameMgr.monsters_P.GetObj();
        newMonster.SetStatus(GameMgr.monsterDatas[Random.Range(0,4)]);
        newMonster.Test = false;
        newMonster.transform.position = monsterSpawnPos.transform.position;
    }


    void OnOffTestCanavs()
    {
        testCanavas.SetActive(!testCanavas.activeSelf);
    }


}
