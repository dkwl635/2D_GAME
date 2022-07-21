using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestMgr : MonoBehaviour
{
    GameMgr GameMgr;
    public HeroCtrl hero;
    public GameObject testCanavas;

    public Button closeBtn;
    public Button skillBtn;
    public Button abilityBtn;

    public Button monsterSpawn_1;
    public Button monsterSpawn_2;

    public GameObject monsterSpawnPos;
    [Header("Skill")]
    public Skill[] skills;
    public GameObject skillCtrlBox;
    public Transform scrollerTr;
    public GameObject skillBtnObj;

    [Header("AbilityPanel")]
    public GameObject AbilityPanel;
    public TMP_InputField attack_Input;
    public TMP_InputField hp_Input;
    public TMP_InputField skPw_Input;
    public TMP_InputField skCool_Input;
    public TMP_InputField def_Input;

   

    private void Start()
    {
        GameMgr = GameMgr.Inst;

        monsterSpawn_1.onClick.AddListener(SpawnMonster_1);
        monsterSpawn_2.onClick.AddListener(SpawnMonster_2);

        closeBtn.onClick.AddListener(AllBoxOff);
        skillBtn.onClick.AddListener(OnSkillCtrlBox);
        abilityBtn.onClick.AddListener(OnAbilitylBox);

        attack_Input.onValueChanged.AddListener(SetAttackPower);
        hp_Input.onValueChanged.AddListener(SetHp);
        skPw_Input.onValueChanged.AddListener(SetSkillPower);
        skCool_Input.onValueChanged.AddListener(SetSkillCool);
        def_Input.onValueChanged.AddListener(SetDef);


        for (int i = 0; i < skills.Length; i++)
        {
            GameObject obj = Instantiate(skillBtnObj, scrollerTr);
            obj.GetComponent<TestSkillBtn>().SetSkill(skills[i]);
        }
        
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

    void OnSkillCtrlBox()
    {
        AllBoxOff();
        skillCtrlBox.SetActive(true);
    }

    void OnAbilitylBox()
    {
        AllBoxOff();
        AbilityPanel.SetActive(true);
    }


    void AllBoxOff()
    {
        skillCtrlBox.SetActive(false);
        AbilityPanel.SetActive(false);
    }

    void SetAttackPower(string str)
    {
        if (string.IsNullOrEmpty(str))
            return;
        int value = int.Parse(str);      
        hero.AttackPower = value;
    }

    void SetHp(string str)
    {
        if (string.IsNullOrEmpty(str))
            return;
        int value = int.Parse(str);
        hero.Hp = value;
     
    }
    void SetSkillPower(string str)
    {
        if (string.IsNullOrEmpty(str))
            return;
        int value = int.Parse(str);
        hero.skillPower = value;
    }

    void SetSkillCool(string str)
    {
        if (string.IsNullOrEmpty(str))
            return;
        int value = int.Parse(str);
        hero.skillCool = value * 0.01f;
    }

    void SetDef(string str)
    {
        if (string.IsNullOrEmpty(str))
            return;
        int value = int.Parse(str);
        hero.def = value;
    }
}
