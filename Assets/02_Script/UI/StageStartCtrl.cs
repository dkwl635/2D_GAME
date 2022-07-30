using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageStartCtrl : MonoBehaviour
{
    public int stageLevel = 0;

    public GameObject monsterCardPanel;
    public GameObject skillCardPanel;

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
        monsterCardPanel.gameObject.SetActive(true);
        skillCardPanel.gameObject.SetActive(false);
        stageLevel = GameMgr.Inst.stageLevel;
        stageData = GameMgr.Inst.stageDatas[stageLevel];
        skills = GameMgr.Inst.skills;

        nextBtn.gameObject.SetActive(true);

        for (int i = 0; i < stageData.monsterDatas.Length; i++)
        {
            monstercards[i].SetCard(stageData.monsterDatas[i].GetCard());
            monstercards[i].gameObject.SetActive(true);
        }
    }

   
    void NextBtn()
    {
        monsterCardPanel.gameObject.SetActive(false);
        skillCardPanel.gameObject.SetActive(true);

        for (int i = 0; i < Skillcards.Length; i++)
        {
            if(skills[i].skill_MaxLv == skills[i].skill_Lv)
            {
                Skillcards[i].gameObject.SetActive(false);
                continue;
            }

            Skillcards[i].SetCard(skills[i].GetCard());
            Skillcards[i].skill = skills[i];
            Skillcards[i].gameObject.SetActive(true);
        }

        nextBtn.gameObject.SetActive(false);
    }


}
