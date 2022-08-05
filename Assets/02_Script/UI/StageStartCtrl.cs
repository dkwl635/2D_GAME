using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CardHelp;
using UnityEngine.UI;

public class StageStartCtrl : MonoBehaviour
{
    public int stageLevel = 0;

    public GameObject monsterCardPanel;
    public GameObject skillCardPanel;

    public TextMeshProUGUI monsterCountTxt;
    public Button nextBtn;

    public Card[] monsterCards;
    public Card bossCard;
    public SkillCard[] Skillcards;

    StageData stageData;
    Skill[] skills;

    private void Start()
    {
        nextBtn.onClick.AddListener(NextBtn);

        for (int i = 0; i < Skillcards.Length; i++)
        {
            Skillcards[i].SetCard(skills[i].GetCard());
            Skillcards[i].skill = skills[i];
            Skillcards[i].gameObject.SetActive(true);
        }
    }

    

    private void OnEnable()
    {
        monsterCardPanel.gameObject.SetActive(true);
        skillCardPanel.gameObject.SetActive(false);
      
        stageData = GameMgr.Inst.StageData;
        skills = GameMgr.Inst.skills;

        nextBtn.gameObject.SetActive(true);

        for (int i = 0; i < stageData.monsterDatas.Length; i++)
        {
            monsterCards[i].SetCard(stageData.monsterDatas[i].GetCard());
            monsterCards[i].gameObject.SetActive(true);
        }

        if (GameMgr.Inst.StageData.bossMonsterPrefab)
        {
            bossCard.SetCard(GameMgr.Inst.StageData.bossMonsterPrefab.GetComponent<SetCard>().GetCard());
            bossCard.gameObject.SetActive(true);
        }

        monsterCountTxt.text = stageData.monsterCount + "¸¶¸®";
    }

    

    void NextBtn()
    {
        monsterCardPanel.gameObject.SetActive(false);
        skillCardPanel.gameObject.SetActive(true);

        
        for (int i = 0; i < Skillcards.Length; i++)
        {
            if (skills[i].skill_MaxLv == skills[i].skill_Lv)
            {
                Skillcards[i].gameObject.SetActive(false);
                continue;
            }
        }
            nextBtn.gameObject.SetActive(false);
    }


}
