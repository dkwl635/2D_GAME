using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    static public GameMgr Inst; //�̱���
    public HeroCtrl hero; //ĳ���� Ŭ����
    //����
    AudioSource audioSource;
    public AudioClip[] audioClips; //ȿ������ �����س���
    //ȿ������ �̸����� ã������ ���� ��ųʸ�
    Dictionary<string, AudioClip> Dic_AudioClip = new Dictionary<string, AudioClip>();

    
    [Header("ObjPool")]//������ƮǮ�� ����ϴ� ������Ʈ��
    public DamageTxtEffect_P DamageTxtEffect_P; //����������� ��ġ�� ��Ÿ����
    public Monsters_P monsters_P;                       //���� ������Ʈ
    public PlayerHitEffect_P playerHitEffect_P;       //�÷��̾� �ǰ� ����Ʈ 
    public Coin_P coin_P;                                     //���� ������Ʈ
    public ExpBall_P expBall_P;                             //����ġ ������Ʈ

    [Header("UI")]
    public GameObject LobbyPanel; //���� �κ� 
    public GameObject InGameUIs; //�� ���� ���۽� ������
    public GameObject lvUpPanel;   //�������� ������ 
    public GameObject GameOverPanel;    //���� ����� ������  
    public GameObject eqInfoBox;    //��� �� ���� â
    public Button gameStartBtn;       //���ӽ��۹�ư
    public Button eqInfoBoxBtn;        //�������� ���¹�ư
    public Button nextBtn;              //���� ���� ���� ��ư       
    public Button resetScoreBtn;    //���ھ� ���¹�ư

    public TextMeshProUGUI monsterKillCountTxt;  //���� ųī��Ʈ
    public TextMeshProUGUI eqInfoTxt;                  //ĳ���� ���� �ؽ�Ʈ
    public TextMeshProUGUI bestScoreTxt;              //�ְ����� �ؽ�Ʈ
 

    [Header("StageData")]
    [SerializeField] private StageData[] stageDatas; //�������� ���� ������ �迭
    public TextMeshProUGUI stageLvTxt;                  //�������� ���� �ؽ�Ʈ
    public GameObject stageInfo;                            //�������� ���۽� ��Ÿ�� UI Panel
    public int stage;                                                //���� ��������
    public static int BestStage;                                 //�ְ��� ��������

    [Header("Skill")] 
    public Skill[] skills; //��ü ��ų���

    [Header("BossMon")]  //�������� ����
    public GameObject[] bossMonster;
    public GameObject bossSpawntxt; //���������� ��Ÿ�� �ؽ�Ʈ

    [Header("StartWeapon")]
    public EquipmentItem startWeapon; //���� �ʱ� ����

   private bool bossSpawn = false; //�������Ͱ� �����Ǿ�����
    [HideInInspector] public bool gameClear = false;

    //MonsterSpawn
    //����
    int monsterkillCount; //���� ų�� 
    int maxMonsterCount; //��ü ����
    float x, y; //��ġ ����
    

    public StageData StageData //���� �������� ������ ����
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
        //�ְ� ��� ���� ��������
        BestStage = PlayerPrefs.GetInt("BestStage", 0);
       
        //��� ���� �ؽ�Ʈ �����ϱ�
        if (BestStage == 0)
        {
            bestScoreTxt.text = "-";
            resetScoreBtn.gameObject.SetActive(false);
        }
        else   
            bestScoreTxt.text = (BestStage / 5 + 1) + "-" + (BestStage % 5 + 1);

        //�����Ŭ���� �̸����� ã���� �ְ� ��ųʸ��� ����
        for (int i = 0; i < audioClips.Length; i++)   
            Dic_AudioClip.Add(audioClips[i].name, audioClips[i]);
        
        gameStartBtn.onClick.AddListener(GameStart);
        eqInfoBoxBtn.onClick.AddListener(OffEqItemInfoBox);
        nextBtn.onClick.AddListener(RoundStart);
        resetScoreBtn.onClick.AddListener(ResetScore);

        //ĳ���� �������� ȣ��� �Լ� ���
        hero.LevelUP_Event += OnLevelUpPanel;
    }

    public void GameStart() //�� ���ӽ���
    {
        LobbyPanel.SetActive(false); //�κ� �г� off
        hero.SetEqItem(startWeapon);  //���� ���� ����
        RoundStart();   //���� ����
    }

    public void RoundStart()//���� ����
    {     
        StageMonsterInfoShow(); //������������ �� ������� �Ұ�
     
        StartCoroutine(MonsterSpawner());   //���� ���� �ڷ�ƾ ����
        InGameUIs.SetActive(true); //InGameUIs On
        nextBtn.gameObject.SetActive(false); //��������� ���� ��ư off
        ShopMgr.Inst.Shop = false;                //���� off
    }

    public void RoundEnd() //���� ��
    {
        //ü�� ȸ��
        hero.Hp = hero.maxHp;

        //��ų ��ü ����
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].getSkill)
                skills[i].SkillRefresh();
        }
        //���� ����
        ShopMgr.Inst.Shop = true;

        stage++;
        if (stage == 5)//���� ���� 5���������̱⶧����
        {
            stage = 4; //1-5 �� �ִ� ��������
            gameClear = true;
            GameOver();
        }

        stageLvTxt.text = (stage/5 + 1) + "-" + (stage % 5 + 1);

        nextBtn.gameObject.SetActive(true); //��������� ���� ��ư on
    }

    void StageMonsterInfoShow() //������������ �� ������� �Ұ�
    {
        Time.timeScale = 0.0f;
        stageInfo.SetActive(true); //������������ �� ������� �Ұ�
        //StageStartCtrl->OnEnable �Լ� ������ ��
    }

    public void StageGameStart()
    {//���� ���� ��ų���ý� �ð��� ������
        Time.timeScale = 1.0f;
        stageInfo.SetActive(false);

        for (int i = 0; i < skills.Length; i++)  //��ų�� ����    
            if (skills[i].getSkill) //������ �ִ� ��ų��
               skills[i].SkillStart();   
    }


    IEnumerator MonsterSpawner() //���� ���� �ڷ�ƾ
    {
        //�ؽ�Ʈ ����
        maxMonsterCount = StageData.monsterCount;
        monsterKillCountTxt.text = "0 / " + maxMonsterCount;
        monsterkillCount = 0; //���� ų��
        
        yield return new WaitForSeconds(2.0f);

        if (stage == 4)//���� 5��° ����� ������ȯ 
            StartCoroutine(BossSpanw());

        int monsterSpawnCount = 0;

        //���� �ֱ� 1.5��
        WaitForSeconds spanwTime = new WaitForSeconds(1.5f);
        while (maxMonsterCount > monsterSpawnCount)//��ǥ �� ����
        {     
            Monster newMonster = monsters_P.GetObj(); //���� ������ƮǮ���� ������
            //������� �� ������ ���� ����
            newMonster.SetStatus(StageData.monsterDatas[Random.Range(0, StageData.monsterDatas.Length)]); 
            //ī�޶� �۷��� ��ġ���� ����
            newMonster.transform.position = RandomSpanw();
            //���� ī��Ʈ++
            monsterSpawnCount++;
            yield return spanwTime; //1.5����
        }
    }

    public Vector3 RandomSpanw()
    {
        Vector3 spawnpos = Vector3.zero; 
        //ī�޶� �� ũ�Ⱑ������
        Vector3  camWMin = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 camWMax = Camera.main.ViewportToWorldPoint(Vector3.one);

        //�¿� ���Ʒ� ���� ����
        int dir = Random.Range(0, 4);

        if(dir == 0) //��
        {
            y = camWMax.y + 2.0f;
            x = Random.Range(camWMin.x, camWMax.x);
        }
        else if(dir == 1) //�Ʒ�
        {
            y = camWMin.y - 2.0f;
            x = Random.Range(camWMin.x, camWMax.x);
        }
        else if(dir == 2) //��
        {
            x = camWMax.x + 2.0f;
            y = Random.Range(camWMin.y, camWMax.y);
        }
        else if (dir == 3) //��
        {
            x = camWMin.x-+ 2.0f;
            y = Random.Range(camWMin.y, camWMax.y);
        }
        //������ġ 
        spawnpos.y = y;
        spawnpos.x = x;
        //�� ����
        return spawnpos;
    }

    public void MonsterKill() //���Ͱ� ������ ȣ��Ǵ�
    {
        monsterkillCount++; //ųī��Ʈ++
        monsterKillCountTxt.text = monsterkillCount + " / " + maxMonsterCount;
        //��ǥ ų�� �޼��� ���� ���� .&& ���������� üũ
        if (monsterkillCount == maxMonsterCount && !bossSpawn)
            RoundEnd();

    }

    public void BossKill() //�������� óġ��
    {
        bossSpawn = false;
        if (monsterkillCount == maxMonsterCount && !bossSpawn)
            RoundEnd();
    }

    public void SpawnExpBall(Vector2 spawnPos, int expValue)
    {//���� óġ�� ȣ�� 
        //������ ��ġ���� ������ ������ ��ġ �� ����
        spawnPos = spawnPos + Random.insideUnitCircle;
        expBall_P.GetObj().SetExpBall(spawnPos, hero, expValue);
    }

    public void SpawnCoin(Vector2 spawnPos)
    {//���� óġ�� ȣ�� 
        //������ ��ġ���� ������ ������ ��ġ �� ����
        spawnPos = spawnPos + Random.insideUnitCircle;
        coin_P.GetObj().SetCoin(spawnPos);
    }

    public void GetCoin(int coin) //���� ������Ʈ�� ���� ���
    {
        SoundEffectPlay("GetCoin"); //ȿ����
        hero.Coin += coin;  //���� �� ����
    }

    void OnLevelUpPanel() //�������� ��Ÿ���� UI
    {
        lvUpPanel.gameObject.SetActive(true);
    }

    public void AddAblilty(AbilityType abilityType , int value) //�ɷ�ġ�� ��ġ�������ִ� �Լ�
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
    {//�������� ������
        ShowEqItemInfo(hero.EquipmentItems[type], pos);
    }

    public void ShowEqItemInfo(EquipmentItem item, Vector2 pos)
    {//�������� ������ �����ִ�
        eqInfoBox.transform.position = pos;
        eqInfoBox.SetActive(true); //������������ �ڽ�
        eqInfoTxt.text = "";
        string str = "";
        EquipmentType type = item.Type;
        str += item.itemName;
        //Ÿ�Ժ� �ɷ�ġ ����
        if (type == EquipmentType.Weapon_L || type == EquipmentType.Weapon_R)
           str += "\n���ݷ� + " + item.value;
        else if (type == EquipmentType.Shield || type == EquipmentType.Armor || type == EquipmentType.Plant)
            str += "\n���� + " + item.value;

        eqInfoTxt.text = str;
    }

    public void OffEqItemInfoBox() //������������ off
    {
        eqInfoBox.SetActive(false);
    }

    IEnumerator BossSpanw() //���� ���� �ڷ�ƾ
    {
        //���� ���� ų���� ��ǥ�� ���̻� ���������� ���
        while(maxMonsterCount/2 > monsterkillCount)   
            yield return null;
     
        //���� ���� �ؽ�Ʈ on
        Vector2 vector2 = bossSpawntxt.transform.position; //������ġ ����
        bossSpawntxt.SetActive(true);

        bool spawn = true;

        float time = 0.0f;
        while (time < 10.0f) //10�ʵ��� 
        {     //�ؽ�Ʈ �������� �̵��ϴ� ȿ�� ����
            time += Time.deltaTime;
            bossSpawntxt.transform.position += Vector3.right;

            if(spawn && time >= 2.5f) //2.5�� �� ���� �����ϱ�
            {
                spawn = false;
                bossSpawn = true;
                GameObject boss = Instantiate(bossMonster[stage/5], RandomSpanw(), Quaternion.identity);
            }

            yield return null;
        }

        //���� ��ġ�� �̵�
        bossSpawntxt.transform.position = vector2;
        //�ؽ�Ʈ ����
        bossSpawntxt.gameObject.SetActive(false);
    }

    public void GameOver()//��������
    {
        //��罺ų ����
        for (int i = 0; i < skills.Length; i++)
            if (skills[i].getSkill)
                skills[i].SkillRefresh();

        GameOverPanel.SetActive(true);
    }


    private void ResetScore() //��� �ʱ�ȭ
    {
        PlayerPrefs.DeleteAll(); //����� ��� �����

        bestScoreTxt.text = "-";
        resetScoreBtn.gameObject.SetActive(false);
    }

    public void SoundEffectPlay(string name) //ȿ���� ����
    {
        //����� ȿ���� ����
        if (Dic_AudioClip.ContainsKey(name))
            audioSource.PlayOneShot(Dic_AudioClip[name]);
    }

}

