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
    AudioSource audioSource;
    public AudioClip[] audioClips;
    Dictionary<string, AudioClip> Dic_AudioClip = new Dictionary<string, AudioClip>();

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
    public GameObject GameOverPanel;
    public GameObject startTxtObj;
    public GameObject eqInfoBox;
    public Button gameStartBtn;
    public Button eqInfoBoxBtn;
    public Button nextBtn;
    public Button resetScoreBtn;

    public TextMeshProUGUI monsterKillCountTxt; 
    public TextMeshProUGUI eqInfoTxt;
    public TextMeshProUGUI bestScoreTxt;
 

    [Header("StageData")]
    [SerializeField] private StageData[] stageDatas;
    public TextMeshProUGUI stageLvTxt;
    public GameObject stageInfo;
    public int stage;
    public static int BestStage;

    public StageData StageData
    {
        get { return stageDatas[stage]; }
    }

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

    public bool bossSpawn = false;
    public bool gameClear = false;


    private void Awake()
    {
        Inst = this;

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {      
        BestStage = PlayerPrefs.GetInt("BestStage", 0);
       
        if (BestStage == 0)
        {
            bestScoreTxt.text = "-";
            resetScoreBtn.gameObject.SetActive(false);
        }
        else   
            bestScoreTxt.text = (BestStage / 5 + 1) + "-" + (BestStage % 5 + 1);

        for (int i = 0; i < audioClips.Length; i++)   
            Dic_AudioClip.Add(audioClips[i].name, audioClips[i]);
        


        gameStartBtn.onClick.AddListener(GameStart);
        eqInfoBoxBtn.onClick.AddListener(OffEqItemInfoBox);
        nextBtn.onClick.AddListener(RoundStart);
        resetScoreBtn.onClick.AddListener(ResetScore);

        heroTr = hero.transform;
        hero.LevelUP_Event += OnLevelUpPanel;
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

        stage++;
        if (stage == 5)//현재 지금 5스테이지이기때문에
        {
            stage = 4; //1-5 가 최대 스테이지
            gameClear = true;
            GameOver();
        }

        stageLvTxt.text = (stage/5 + 1) + "-" + (stage % 5 + 1);

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
        maxMonsterCount = StageData.monsterCount;
        monsterKillCountTxt.text = "0 / " + maxMonsterCount;
        monsterkillCount = 0;

        yield return new WaitForSeconds(2.0f);

        if (stage == 4)
            StartCoroutine(BossSpanw());

        int monsterSpawnCount = 0;

        WaitForSeconds spanwTime = new WaitForSeconds(1.5f);
        while (maxMonsterCount > monsterSpawnCount)
        {
            yield return spanwTime;
            Monster newMonster = monsters_P.GetObj();
            newMonster.SetStatus(StageData.monsterDatas[Random.Range(0, StageData.monsterDatas.Length)]);
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

        if (monsterkillCount == maxMonsterCount && !bossSpawn)
            RoundEnd();

    }

    public void BossKill()
    {
        GameMgr.Inst.bossSpawn = false;

        if (monsterkillCount == maxMonsterCount && !bossSpawn)
            RoundEnd();
    }

    public void SpawnExpBall(Vector2 spawnPos, int expValue)
    {
        spawnPos = spawnPos + Random.insideUnitCircle;
            expBall_P.GetObj().SetExpBall(spawnPos, hero, expValue);
    }

    public void SpawnCoin(Vector2 spawnPos)
    {
        spawnPos = spawnPos + Random.insideUnitCircle * 2;
        coin_P.GetObj().SetCoin(spawnPos);
    }

    public void GetCoin(int coin)
    {
        SoundEffectPlay("GetCoin");
        hero.Coin += coin;
    }

    void OnLevelUpPanel()
    {
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
        str += hero.EquipmentItems[type].itemName;
        if (type == EquipmentType.Weapon_L || type == EquipmentType.Weapon_R)
        {
            str += "\n공격력 + " + hero.EquipmentItems[type].value;
        }
        else if (type == EquipmentType.Shield || type == EquipmentType.Armor || type == EquipmentType.Plant)
        {
            str += "\n방어력 + " + hero.EquipmentItems[type].value;
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
        while(maxMonsterCount/2 > monsterkillCount)
        {
            yield return null;
        }


        Vector2 vector2 = bossSpawntxt.transform.position;
        bossSpawntxt.SetActive(true);

        bool spawn = true;

        float time = 0.0f;
        while (time < 10.0f)
        {
            time += Time.deltaTime;
            bossSpawntxt.transform.position += Vector3.right;

            if(spawn && time >= 2.5f)
            {
                spawn = false;
                bossSpawn = true;
                GameObject boss = Instantiate(bossMonster[stage/5], RandomSpanw(), Quaternion.identity);
            }

            yield return null;
        }
        bossSpawntxt.transform.position = vector2;
        bossSpawntxt.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].getSkill)
                skills[i].SkillRefresh();
        }

        GameOverPanel.SetActive(true);
    }


    private void ResetScore()
    {
        PlayerPrefs.DeleteAll();

        bestScoreTxt.text = "-";
        resetScoreBtn.gameObject.SetActive(false);
    }

    public void SoundEffectPlay(string name)
    {
        if (Dic_AudioClip.ContainsKey(name))
            audioSource.PlayOneShot(Dic_AudioClip[name]);
    }

}

