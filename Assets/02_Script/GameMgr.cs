using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    static public GameMgr Inst; //싱글톤
    public HeroCtrl hero; //캐릭터 클레스
    //사운드
    AudioSource audioSource;
    public AudioClip[] audioClips; //효과음을 저장해놓는
    //효과음을 이름별로 찾기위해 만든 딕셔너리
    Dictionary<string, AudioClip> Dic_AudioClip = new Dictionary<string, AudioClip>();

    
    [Header("ObjPool")]//오브젝트풀을 사용하는 오브젝트들
    public DamageTxtEffect_P DamageTxtEffect_P; //데미지적용시 수치를 나타내는
    public Monsters_P monsters_P;                       //몬스터 오브젝트
    public PlayerHitEffect_P playerHitEffect_P;       //플레이어 피격 이펙트 
    public Coin_P coin_P;                                     //코인 오브젝트
    public ExpBall_P expBall_P;                             //경험치 오브젝트

    [Header("UI")]
    public GameObject LobbyPanel; //시작 로비 
    public GameObject InGameUIs; //본 게임 시작시 나오는
    public GameObject lvUpPanel;   //레벨업시 나오는 
    public GameObject GameOverPanel;    //게임 종료시 나오는  
    public GameObject eqInfoBox;    //장비 및 정보 창
    public Button gameStartBtn;       //게임시작버튼
    public Button eqInfoBoxBtn;        //장비및정보 오픈버튼
    public Button nextBtn;              //다음 라운드 시작 버튼       
    public Button resetScoreBtn;    //스코어 리셋버튼

    public TextMeshProUGUI monsterKillCountTxt;  //몬스터 킬카운트
    public TextMeshProUGUI eqInfoTxt;                  //캐릭터 정보 텍스트
    public TextMeshProUGUI bestScoreTxt;              //최고점수 텍스트
 

    [Header("StageData")]
    [SerializeField] private StageData[] stageDatas; //스테이지 정보 데이터 배열
    public TextMeshProUGUI stageLvTxt;                  //스태이지 라운드 텍스트
    public GameObject stageInfo;                            //스테이지 시작시 나타날 UI Panel
    public int stage;                                                //현재 스테이지
    public static int BestStage;                                 //최고기록 스테이지

    [Header("Skill")] 
    public Skill[] skills; //전체 스킬목록

    [Header("BossMon")]  //보스몬스터 정보
    public GameObject[] bossMonster;
    public GameObject bossSpawntxt; //보스스폰시 나타날 텍스트

    [Header("StartWeapon")]
    public EquipmentItem startWeapon; //시작 초기 무기

   private bool bossSpawn = false; //보스몬스터가 스폰되었는지
    [HideInInspector] public bool gameClear = false;

    //MonsterSpawn
    //계산용
    int monsterkillCount; //몬스터 킬수 
    int maxMonsterCount; //전체 몬스터
    float x, y; //위치 계산용
    

    public StageData StageData //현재 스테이지 정보를 리턴
    {
        get { return stageDatas[stage]; }
    }
    private void Awake()
    {
        Inst = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {     
        //최고 기록 정보 가져오기
        BestStage = PlayerPrefs.GetInt("BestStage", 0);
       
        //기록 점수 텍스트 셋팅하기
        if (BestStage == 0)
        {
            bestScoreTxt.text = "-";
            resetScoreBtn.gameObject.SetActive(false);
        }
        else   
            bestScoreTxt.text = (BestStage / 5 + 1) + "-" + (BestStage % 5 + 1);

        //오디오클립을 이름별로 찾을수 있게 딕셔너리에 저장
        for (int i = 0; i < audioClips.Length; i++)   
            Dic_AudioClip.Add(audioClips[i].name, audioClips[i]);
        
        gameStartBtn.onClick.AddListener(GameStart);
        eqInfoBoxBtn.onClick.AddListener(OffEqItemInfoBox);
        nextBtn.onClick.AddListener(RoundStart);
        resetScoreBtn.onClick.AddListener(ResetScore);

        //캐릭터 레벨업시 호출될 함수 등록
        hero.LevelUP_Event += OnLevelUpPanel;
    }

    public void GameStart() //본 게임시작
    {
        LobbyPanel.SetActive(false); //로비 패널 off
        hero.SetEqItem(startWeapon);  //시작 무기 장착
        RoundStart();   //라운드 시작
    }

    public void RoundStart()//라운드 시작
    {     
        StageMonsterInfoShow(); //스테이지정보 및 등장몬스터 소개
     
        StartCoroutine(MonsterSpawner());   //몬스터 스폰 코루틴 실행
        InGameUIs.SetActive(true); //InGameUIs On
        nextBtn.gameObject.SetActive(false); //다음라운드로 가는 버튼 off
        ShopMgr.Inst.Shop = false;                //상점 off
    }

    public void RoundEnd() //라운드 끝
    {
        //체력 회복
        hero.Hp = hero.maxHp;

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

        nextBtn.gameObject.SetActive(true); //다음라운드로 가는 버튼 on
    }

    void StageMonsterInfoShow() //스테이지정보 및 등장몬스터 소개
    {
        Time.timeScale = 0.0f;
        stageInfo.SetActive(true); //스테이지정보 및 등장몬스터 소개
        //StageStartCtrl->OnEnable 함수 실행이 됨
    }

    public void StageGameStart()
    {//라운드 마다 스킬선택시 시간이 움직임
        Time.timeScale = 1.0f;
        stageInfo.SetActive(false);

        for (int i = 0; i < skills.Length; i++)  //스킬들 실행    
            if (skills[i].getSkill) //가지고 있는 스킬만
               skills[i].SkillStart();   
    }


    IEnumerator MonsterSpawner() //몬스터 스폰 코루틴
    {
        //텍스트 셋팅
        maxMonsterCount = StageData.monsterCount;
        monsterKillCountTxt.text = "0 / " + maxMonsterCount;
        monsterkillCount = 0; //현재 킬수
        
        yield return new WaitForSeconds(2.0f);

        if (stage == 4)//만약 5번째 라운드면 보스소환 
            StartCoroutine(BossSpanw());

        int monsterSpawnCount = 0;

        //스폰 주기 1.5초
        WaitForSeconds spanwTime = new WaitForSeconds(1.5f);
        while (maxMonsterCount > monsterSpawnCount)//목표 수 까지
        {     
            Monster newMonster = monsters_P.GetObj(); //몬스터 오브젝트풀에서 가져옴
            //등장몬스터 중 무작위 종류 스폰
            newMonster.SetStatus(StageData.monsterDatas[Random.Range(0, StageData.monsterDatas.Length)]); 
            //카메라 밖랜덤 위치에서 스폰
            newMonster.transform.position = RandomSpanw();
            //스폰 카운트++
            monsterSpawnCount++;
            yield return spanwTime; //1.5초후
        }
    }

    public Vector3 RandomSpanw()
    {
        Vector3 spawnpos = Vector3.zero; 
        //카메라 뷰 크기가져오기
        Vector3  camWMin = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 camWMax = Camera.main.ViewportToWorldPoint(Vector3.one);

        //좌우 위아래 랜덤 선택
        int dir = Random.Range(0, 4);

        if(dir == 0) //위
        {
            y = camWMax.y + 2.0f;
            x = Random.Range(camWMin.x, camWMax.x);
        }
        else if(dir == 1) //아래
        {
            y = camWMin.y - 2.0f;
            x = Random.Range(camWMin.x, camWMax.x);
        }
        else if(dir == 2) //우
        {
            x = camWMax.x + 2.0f;
            y = Random.Range(camWMin.y, camWMax.y);
        }
        else if (dir == 3) //좌
        {
            x = camWMin.x-+ 2.0f;
            y = Random.Range(camWMin.y, camWMax.y);
        }
        //스폰위치 
        spawnpos.y = y;
        spawnpos.x = x;
        //값 리턴
        return spawnpos;
    }

    public void MonsterKill() //몬스터가 죽을시 호출되는
    {
        monsterkillCount++; //킬카운트++
        monsterKillCountTxt.text = monsterkillCount + " / " + maxMonsterCount;
        //목표 킬수 달성시 라운드 종료 .&& 보스스폰이 체크
        if (monsterkillCount == maxMonsterCount && !bossSpawn)
            RoundEnd();

    }

    public void BossKill() //보스몬스터 처치시
    {
        bossSpawn = false;
        if (monsterkillCount == maxMonsterCount && !bossSpawn)
            RoundEnd();
    }

    public void SpawnExpBall(Vector2 spawnPos, int expValue)
    {//몬스터 처치시 호출 
        //지정된 위치에서 원으로 랜덤한 위치 에 스폰
        spawnPos = spawnPos + Random.insideUnitCircle;
        expBall_P.GetObj().SetExpBall(spawnPos, hero, expValue);
    }

    public void SpawnCoin(Vector2 spawnPos)
    {//몬스터 처치시 호출 
        //지정된 위치에서 원으로 랜덤한 위치 에 스폰
        spawnPos = spawnPos + Random.insideUnitCircle;
        coin_P.GetObj().SetCoin(spawnPos);
    }

    public void GetCoin(int coin) //코인 오브젝트를 먹을 경우
    {
        SoundEffectPlay("GetCoin"); //효과음
        hero.Coin += coin;  //코인 값 적용
    }

    void OnLevelUpPanel() //레벨업시 나타나는 UI
    {
        lvUpPanel.gameObject.SetActive(true);
    }

    public void AddAblilty(AbilityType abilityType , int value) //능력치별 수치적용해주는 함수
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
                    hero.attackPower += value;               
                break;
            case AbilityType.Def:              
                    hero.def += value;         
                break;
            case AbilityType.SkillPw:          
                    hero.skillPower += value;         
                break;
            case AbilityType.SkillCool:          
                    hero.SkillCool -= value;           
                break;
            default:
                break;
        }
    }
    public void ShowEqItemInfo(EquipmentType type, Vector2 pos) 
    {//장착중인 아이템
        ShowEqItemInfo(hero.EquipmentItems[type], pos);
    }

    public void ShowEqItemInfo(EquipmentItem item, Vector2 pos)
    {//장비아이템 정보를 보여주는
        eqInfoBox.transform.position = pos;
        eqInfoBox.SetActive(true); //장비아이템정보 박스
        eqInfoTxt.text = "";
        string str = "";
        EquipmentType type = item.Type;
        str += item.itemName;
        //타입별 능력치 적용
        if (type == EquipmentType.Weapon_L || type == EquipmentType.Weapon_R)
           str += "\n공격력 + " + item.value;
        else if (type == EquipmentType.Shield || type == EquipmentType.Armor || type == EquipmentType.Plant)
            str += "\n방어력 + " + item.value;

        eqInfoTxt.text = str;
    }

    public void OffEqItemInfoBox() //장비아이템정보 off
    {
        eqInfoBox.SetActive(false);
    }

    IEnumerator BossSpanw() //보스 스폰 코루틴
    {
        //현재 몬스터 킬수가 목표의 반이상 잡을때까지 대기
        while(maxMonsterCount/2 > monsterkillCount)   
            yield return null;
     
        //보스 등장 텍스트 on
        Vector2 vector2 = bossSpawntxt.transform.position; //원래위치 저장
        bossSpawntxt.SetActive(true);

        bool spawn = true;

        float time = 0.0f;
        while (time < 10.0f) //10초동안 
        {     //텍스트 우측으로 이동하는 효과 실행
            time += Time.deltaTime;
            bossSpawntxt.transform.position += Vector3.right;

            if(spawn && time >= 2.5f) //2.5초 후 보스 스폰하기
            {
                spawn = false;
                bossSpawn = true;
                GameObject boss = Instantiate(bossMonster[stage/5], RandomSpanw(), Quaternion.identity);
            }

            yield return null;
        }

        //원래 위치로 이동
        bossSpawntxt.transform.position = vector2;
        //텍스트 끄기
        bossSpawntxt.gameObject.SetActive(false);
    }

    public void GameOver()//게임종료
    {
        //모든스킬 정지
        for (int i = 0; i < skills.Length; i++)
            if (skills[i].getSkill)
                skills[i].SkillRefresh();

        GameOverPanel.SetActive(true);
    }


    private void ResetScore() //기록 초기화
    {
        PlayerPrefs.DeleteAll(); //저장된 기록 지우기

        bestScoreTxt.text = "-";
        resetScoreBtn.gameObject.SetActive(false);
    }

    public void SoundEffectPlay(string name) //효과음 실행
    {
        //저장된 효과음 실행
        if (Dic_AudioClip.ContainsKey(name))
            audioSource.PlayOneShot(Dic_AudioClip[name]);
    }

}

