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
    public TestSkillBtn TestSkillBtn;

   

    private void Start()
    {
        GameMgr = GameMgr.Inst;

        monsterSpawn_1.onClick.AddListener(SpawnMonster_1);
        monsterSpawn_2.onClick.AddListener(SpawnMonster_2);


        TestSkillBtn.SetSkill(skills[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            OnOffTestCanavs();
    }

    public void SpawnMonster_1()    //����ƺ� ����
    {
        Monster newMonster = GameMgr.monsters_P.GetObj();
        newMonster.SetStatus(GameMgr.monsterDatas[0]);
        newMonster.Test = true;
        newMonster.transform.position = monsterSpawnPos.transform.position;
    }

    public void SpawnMonster_2()    //������¸��� ����
    {
        Monster newMonster = GameMgr.monsters_P.GetObj();
        newMonster.SetStatus(GameMgr.monsterDatas[0]);
        newMonster.Test = false;
        newMonster.transform.position = monsterSpawnPos.transform.position;
    }


    void OnOffTestCanavs()
    {
        testCanavas.SetActive(!testCanavas.activeSelf);
    }


}