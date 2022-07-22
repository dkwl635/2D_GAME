using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageStartCtrl : MonoBehaviour
{
    public int stageLevel = 0;

    public GameObject monsterCard;
    public GameObject SkillCard;

    public Button nextBtn;

    public Card[] monstercards;
    public SkillCard[] Skillcards;

    StageData stageData;
    Skill[] skills;

    private void Start()
    {
        nextBtn.onClick.AddListener(NextBtn);
    }

    private void OnEnable()
    {
        monsterCard.gameObject.SetActive(true);
        SkillCard.gameObject.SetActive(false);
        stageData = GameMgr.Inst.stageDatas[stageLevel];
        stageLevel = GameMgr.Inst.stageLevel;
        skills = GameMgr.Inst.skills;



        for (int i = 0; i < stageData.monsterDatas.Length; i++)
        {
            monstercards[i].SetCard(stageData.monsterDatas[i].GetCard());
            monstercards[i].gameObject.SetActive(true);
        }
    }

   
    void NextBtn()
    {
        monsterCard.gameObject.SetActive(false);
        SkillCard.gameObject.SetActive(true);

        for (int i = 0; i < Skillcards.Length; i++)
        {
            Skillcards[i].SetCard(skills[i].GetCard());
            Skillcards[i].skill = skills[i];
            Skillcards[i].gameObject.SetActive(true);
        }

        nextBtn.gameObject.SetActive(false);
    }


}
