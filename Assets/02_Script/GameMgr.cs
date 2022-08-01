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
    public GameObject LobbyPanel;
    public GameObject InGameUIs;
    public GameObject lvUpPanel;
    public GameObject startTxtObj;
    public GameObject eqInfoBox;
    public Button gameStartBtn;
    public Button eqInfoBoxBtn;
    public Button nextBtn;

    public TextMeshProUGUI monsterKillCountTxt; 
    public TextMeshProUGUI eqInfoTxt;
 

    [Header("StageData")]
    public TextMeshProUGUI stageLvTxt;
    public StageData[] stageDatas;
    public GameObject stageInfo;
    public int stageLevel;
    public int round;

    [Header("Skill")]
    public Skill[] skills;

    [Header("BossMon")]
    public GameObject[] bossMonster;
    public GameObject bossSpawntxt;

    //MonsterSpawn
    //계산용
    int monsterkillCount;
    int maxMonsterCount;
    float x;
    float y;

    public EquipmentItem startWeapon;
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

        gameStartBtn.onClick.AddListener(GameStart);
        eqInfoBoxBtn.onClick.AddListener(OffEqItemInfoBox);
        nextBtn.onClick.AddListener(RoundStart);
    }

    public void GameStart()
    {
        LobbyPanel.SetActive(false);
        hero.SetEqItem(startWeapon);
        RoundStart();
    }

    public void RoundStart()
    {     
        StageMonsterInfoShow();
        StartCoroutine(MonsterSpawner());
        InGameUIs.SetActive(true);
        nextBtn.gameObject.SetActive(false);
        ShopMgr.Inst.Shop = false;
        
    }

    public void RoundEnd()
    {
        //스킬 전체 종료
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].getSkill)
                skills[i].SkillRefresh();
        }
        //상점 등장
        ShopMgr.Inst.Shop = true;

        stageLevel++;
        stageLvTxt.text = "1-" + (stageLevel + 1);

        nextBtn.gameObject.SetActive(true);
    }

    void StageMonsterInfoShow()
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
        monsterkillCount = 0;

        if(stageLevel == 0)
        {
            StartCoroutine(BossSpanw());
        }

        //test
        maxMonsterCount = 1;
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
        monsterKillCountTxt.text = monsterkillCount + " / " + maxMonsterCount;

        if (monsterkillCount == maxMonsterCount)
            RoundEnd();

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
                    hero.attackPower += value;
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

    IEnumerator BossSpanw()
    {
        Vector2 vector2 = bossSpawntxt.transform.position;
        bossSpawntxt.SetActive(true);

        bool spawn = true;

        float time = 0.0f;
        while (time < 10.0f)
        {
            time += Time.deltaTime;
            bossSpawntxt.transform.position += Vector3.right;

            if(spawn && time >= 1.5f)
            {
                spawn = false;
                GameObject boss = Instantiate(bossMonster[round], RandomSpanw(), Quaternion.identity);
            }

            yield return null;
        }
        bossSpawntxt.transform.position = vector2;
        bossSpawntxt.gameObject.SetActive(false);
    }
  

}

