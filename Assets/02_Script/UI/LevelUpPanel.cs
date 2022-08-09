using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CardHelp;

public class LevelUpPanel : MonoBehaviour
{//레벨업시 나타날 UI Panel
    public static LevelUpPanel inst;
 
    [Header("LvUpLable")]
    public TextMeshProUGUI lable;
    public Animation lableAnimation;

    [Header("LvUpObj")]
    public GameObject[] lvUpObjs; //레벨업이 가능한 스킬목록
    public Ability[] abilities;     //레벨업이 가능한 능력치목록

    List<ICardLvUp> skillCardList = new List<ICardLvUp>(); //스킬카드 목록
    List<ICardLvUp> abilityCardList = new List<ICardLvUp>(); //능력치카드 목록
    List<ICardLvUp> skillLvUpAbleList = new List<ICardLvUp>();//최종 레벨업이 가능한 목록

     [Header("LvUpCard")]
    public LvUpCard[] lvUpCard; //레벨업을 시켜주는 카드UI
   
    //Time 계산을 위해
    float realTimeDalta = 0.0f;
    float animationTime = 0.0f;

    private void Awake()
    {
        inst = this;
        //목록 채워주기
        for (int i = 0; i < lvUpObjs.Length; i++)
            skillCardList.Add(lvUpObjs[i].GetComponent<ICardLvUp>());
       
        for (int i = 0; i < abilities.Length; i++)
            abilityCardList.Add(abilities[i]);
    }

    private void Update()
    {
        //이 오브젝트가 켜져있을때는   Time.timeScale = 0이기 떄문에
        //시간계산을 해주어 애니메이션을 돌려준다.
        float curTime = Time.realtimeSinceStartup;
        float deltaTime = curTime - realTimeDalta;
        realTimeDalta = curTime;

        animationTime += deltaTime;

        LableTxt_Update();
    }

    private void OnEnable()
    {
        CheckLevelPossible(); //레벨업이 가능한 것들을 체크한다.
        SetLvUpCard();          //카드를 셋팅해준다.
        Time.timeScale = 0.0f;  //일시정지
    }


    public void OffPanel()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f; //다시 시간 돌려놓기
    }

    void LableTxt_Update()
    {
        //집적 시간을 계산하여 애니메이션을 돌려준다.
        AnimationState a = lableAnimation["LvUpTxt"];
        a.normalizedTime = animationTime % a.length;
    }

    void CheckLevelPossible() //레벨업이 가능한지 체크
    {
        skillLvUpAbleList.Clear(); //리스트 초기화
        for (int i = 0; i < skillCardList.Count; i++)
        {
            if (skillCardList[i].LevelPossible() == true) //레벨업이 가능한지 체크 후
                skillLvUpAbleList.Add(skillCardList[i]);      //리스트에 넣어준다.
        }
    }

    void SetLvUpCard() //레벨업 카드 셋팅
    {
        int idx = 0; //4개의 카드선택 가능 
        List<int> random = new List<int>(); //무작위 선정을 위해
        if(skillLvUpAbleList.Count < 3) //스킬이 3개 미만일경우
        {
            //가지고 있는 스킬 모두 선택가능
            for (int i = 0; i < skillLvUpAbleList.Count; i++) 
                lvUpCard[i].SetCard(skillLvUpAbleList[i]);

            idx += skillLvUpAbleList.Count;
        }
        else //스킬이 3개 이상일경우
        {
            while (random.Count <= 2) //2개까지 랜덤으로 선정
            {
                int a = Random.Range(0, skillLvUpAbleList.Count);
                if (random.Contains(a))
                    continue;
                else
                    random.Add(a);
            }
            //선정된 순서에 맞는 스킬을 셋팅
            for (int i = 0; i < random.Count; i++)
            {
                lvUpCard[i].SetCard(skillLvUpAbleList[random[i]]);
                idx++;
            }
        }
        //스킬선정 후 능력치 선정
        random.Clear(); 
        while (random.Count < 4 - idx) //중복되지 않게 랜덤
        {
            int a = Random.Range(0, abilityCardList.Count);
            if (random.Contains(a))
                continue;
            else
                random.Add(a);
        }
   
        for (int i = 0; i < random.Count; i++)  //선정된 순서에 맞는 능력치을 셋팅
            lvUpCard[idx + i].SetCard(abilityCardList[random[i]]);
        
    }
}
