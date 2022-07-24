using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CardHelp;

public class LevelUpPanel : MonoBehaviour
{
    public static LevelUpPanel inst;

    HeroCtrl hero;
    [Header("LvUpLable")]
    public TextMeshProUGUI lable;
    public Animation lableAnimation;

    [Header("LvUpObj")]
    public GameObject[] lvUpObjs;
    List<ICardLvUp> cardList = new List<ICardLvUp>();

    [Header("LvUpCard")]
    public LvUpCard[] lvUpCard;

    public Ability[] abilities;

    float realTimeDalta = 0.0f;
    float animationTime = 0.0f;

    private void Awake()
    {
        inst = this;

        hero = GameMgr.Inst.hero;

        for (int i = 0; i < lvUpObjs.Length; i++)
        {
            cardList.Add(lvUpObjs[i].GetComponent<ICardLvUp>());
        }
        Debug.Log(cardList.Count);
        for (int i = 0; i < abilities.Length; i++)
        {
            cardList.Add(abilities[i]);
            Debug.Log(cardList.Count);
        }
      
    }

    private void Start()
    {
      
    }


    
    private void Update()
    {
        float curTime = Time.realtimeSinceStartup;
        float deltaTime = curTime - realTimeDalta;
        realTimeDalta = curTime;

        animationTime += deltaTime;

        LableTxt_Update();
    }
    private void OnEnable()
    {      
        CheckLevelPossible();
        SetLvUpCard();
        Time.timeScale = 0.0f;
    }


    public void OffPanel()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void LableTxt_Update()
    {
        AnimationState a = lableAnimation["LvUpTxt"];  
        a.normalizedTime = animationTime % a.length;
    }




    void CheckLevelPossible()
    {
        for (int i = 0; i < cardList.Count;)
        {         
   
            if (cardList[i].LevelPossible() == false) //레벨업이 가능한지 체크 후 제거
                cardList.RemoveAt(i);
            else
                i++;
        }
    }

    void SetLvUpCard()
    {     
        //레벨업이 3개이하 일경우
        if(cardList.Count <= 3)
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                lvUpCard[i].SetCard(cardList[i]);
            }
        }
        else
        {
            List<int> random = new List<int>();
            while(random.Count <3)
            {
                int a = Random.Range(0, cardList.Count);
                if (random.Contains(a))
                    continue;
                else
                    random.Add(a);
            }

            for (int i = 0; i < random.Count; i++)
            {
                Debug.Log(random[i]);   
                lvUpCard[i].SetCard(cardList[random[i]]);
            }


        }
    }

}
