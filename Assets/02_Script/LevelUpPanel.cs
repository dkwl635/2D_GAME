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
    public Ability[] abilities;
    List<ICardLvUp> skillCardList = new List<ICardLvUp>();
    List<ICardLvUp> abilityCardList = new List<ICardLvUp>();

    [Header("LvUpCard")]
    public LvUpCard[] lvUpCard;
   

    float realTimeDalta = 0.0f;
    float animationTime = 0.0f;

    private void Awake()
    {
        inst = this;

        hero = GameMgr.Inst.hero;

        for (int i = 0; i < lvUpObjs.Length; i++)
        {
            skillCardList.Add(lvUpObjs[i].GetComponent<ICardLvUp>());
        }

        for (int i = 0; i < abilities.Length; i++)
        {
            abilityCardList.Add(abilities[i]);
        }

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
        for (int i = 0; i < skillCardList.Count;)
        {
            if (skillCardList[i].LevelPossible() == false) //레벨업이 가능한지 체크 후 제거
                skillCardList.RemoveAt(i);
            else
                i++;
        }
    }

    void SetLvUpCard()
    {
        int count = 3;
        List<int> random = new List<int>();
    
        if(skillCardList.Count < 3)
        {
            for (int i = 0; i < skillCardList.Count; i++)
            {
                lvUpCard[i].SetCard(skillCardList[i]);
                count--;
            }
        }
        else
        {
            while (random.Count < 2)
            {
                int a = Random.Range(0, skillCardList.Count);
                if (random.Contains(a))
                    continue;
                else
                    random.Add(a);
            }

            for (int i = 0; i < random.Count; i++)
            {
                lvUpCard[i].SetCard(skillCardList[random[i]]);
                count--;
            }
        }

        random.Clear();
        while (random.Count < count)
        {
            int a = Random.Range(0, abilityCardList.Count);
            if (random.Contains(a))
                continue;
            else
                random.Add(a);

        }

        int num = 3 - random.Count;
        for (int i = 0; i < random.Count; i++)
        {
            lvUpCard[num + i].SetCard(abilityCardList[random[i]]);
            count--;
        }

    }
}
