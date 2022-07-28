using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    static public GameMgr Inst;
    public HeroCtrl hero;
    Transform heroTr;

    [Header("ObjPool")]
    public DamageTxtEffect DamageTxtEffect;
    public Monsters_P monsters_P;
    public PlayerHitEffect_P playerHitEffect_P;
    public Coin_P coin_P;
    public ExpBall_P expBall_P;

    public MonsterData[] monsterDatas;

    [Header("UI")]
    public TextMeshProUGUI monsterKillCountTxt;
    public GameObject startTxtObj;
    public GameObject lvUpPanel;
    public Button eqInfoBoxBtn;
    public GameObject eqInfoBox;
    public TextMeshProUGUI eqInfoTxt;

    [Header("StageData")]
    public StageData[] stageDatas;
    public GameObject stageInfo;
    public int stageLevel;

    [Header("Skill")]
    public Skill[] skills;
    
    //MonsterSpawn
    //계산용
    int monsterkillCount;
    int maxMonsterCount;
    float x;
    float y;

    public bool Test = false;

  

    private void Awake()
    {
        Inst = this;

    }

    private void Start()
    {
        heroTr = hero.transform;
        hero.LevelUP_Event += OnLevelUpPanel;   

        if (Test) //테스트용 방지
            return;

        StageStart();
        StartCoroutine(MonsterSpawner());
     
        eqInfoBoxBtn.onClick.AddListener(OffEqItemInfoBox);
    }

    

    void StageStart()
    {
        Time.timeScale = 0.0f;
        stageInfo.SetActive(true);
    }
    public void StageGameStart()
    {
        Time.timeScale = 1.0f;
        stageInfo.SetActive(false);

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].getSkill)
                skills[i].SkillStart();
        }

    }


    IEnumerator MonsterSpawner()
    {
        WaitForSeconds spanwTime = new WaitForSeconds(2.0f);
        maxMonsterCount = stageDatas[stageLevel].monsterCount;
        monsterKillCountTxt.text = "0 / " + maxMonsterCount;

        int monsterSpawnCount = 0;
        while (maxMonsterCount > monsterSpawnCount)
        {
            yield return spanwTime;
            Monster newMonster = monsters_P.GetObj();
            newMonster.SetStatus(stageDatas[stageLevel].monsterDatas[Random.Range(0, stageDatas[stageLevel].monsterDatas.Length)]);
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

    public void SpawnExpBall(Vector2 spawnPos, int expValue)
    {
        expBall_P.GetObj().SetExpBall(spawnPos, hero, expValue);
    }

    public void SpawnCoin(Vector2 spawnPos)
    {
        spawnPos = spawnPos + Random.insideUnitCircle * 2;

        coin_P.GetObj().SetCoin(spawnPos);
    }

    public void GetCoin(int coin)
    {
        hero.Coin += coin;
    }

    void OnLevelUpPanel()
    {
        if (Test)
            return;

        lvUpPanel.gameObject.SetActive(true);

    }

    public void AddAblilty(AbilityType abilityType , int value)
    {
        switch (abilityType)
        {
            case AbilityType.Hp:
                {
                    hero.maxHp += value;
                    hero.Hp += value;
                }
                break;
            case AbilityType.AttackPw:
                {
                    hero.AttackPower += value;
                }
                break;
            case AbilityType.Def:
                {
                    hero.def += value;
                }
                break;
            case AbilityType.SkillPw:
                {
                    hero.skillPower += value;
                }
                break;
            case AbilityType.SkillCool:
                {
                    hero.SkillCool -= value;
                }
                break;
            default:
                break;
        }
    }
    public void ShowEqItemInfo(EquipmentType type, Vector2 pos)
    {
        eqInfoBox.transform.position = pos;
        eqInfoBox.SetActive(true);
        eqInfoTxt.text = "";
        string str = "";
        str += hero.equipmentItems[type].itemName;
        if (type == EquipmentType.Weapon_L || type == EquipmentType.Weapon_R)
        {
            str += "\n공격력 + " + hero.equipmentItems[type].value;
        }
        else if (type == EquipmentType.Shield || type == EquipmentType.Armor || type == EquipmentType.Plant)
        {
            str += "\n방어력 + " + hero.equipmentItems[type].value;
        }

        eqInfoTxt.text = str;

    }

    public void ShowEqItemInfo(EquipmentItem item, Vector2 pos)
    {
        eqInfoBox.transform.position = pos;
        eqInfoBox.SetActive(true);
        eqInfoTxt.text = "";
        string str = "";
        EquipmentType type = item.Type;
        str += item.itemName;
        if (type == EquipmentType.Weapon_L || type == EquipmentType.Weapon_R)
        {
            str += "\n공격력 + " + item.value;
        }
        else if (type == EquipmentType.Shield || type == EquipmentType.Armor || type == EquipmentType.Plant)
        {
            str += "\n방어력 + " + item.value;
        }

        eqInfoTxt.text = str;

    }


    public void OffEqItemInfoBox()
    {
        eqInfoBox.SetActive(false);
    }

}
