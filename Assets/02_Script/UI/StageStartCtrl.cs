using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CardHelp;
using UnityEngine.UI;

public class StageStartCtrl : MonoBehaviour
{//스테이지 시작할시 나타나는 
    [Header("UI")]
    public GameObject monsterCardPanel; //등장몬스터를 보여주는
    public GameObject skillCardPanel;       //스킬 선택하는 

    public TextMeshProUGUI monsterCountTxt; //등장몬스터 마리수txt
    public Button nextBtn;                              //다음으로 넘어가는
    [Header("Card_UI")]
    public Card[] monsterCards; //몬스터카드 
    public Card bossCard;           //보스카드
    public SkillCard[] Skillcards;  //스킬카드

    StageData stageData;           //현재 스테이지 정보
    Skill[] skills;                         //스킬목록

    private void Awake()
    {//스킬 목록 가져오기
        skills = GameMgr.Inst.skills;
    }

    private void Start()
    {
        nextBtn.onClick.AddListener(NextBtn);

        //스킬 셋팅해놓기
        for (int i = 0; i < Skillcards.Length; i++)
            Skillcards[i].skill = skills[i];            
    }
    private void OnEnable()
    {//오픈시
        //패널정리
        monsterCardPanel.gameObject.SetActive(true);
        skillCardPanel.gameObject.SetActive(false);
        //현재 스테이지 정보 가져오기
        stageData = GameMgr.Inst.StageData;
        //버튼 활성화
        nextBtn.gameObject.SetActive(true);
        //등장몬스터 카드 셋팅하기
        for (int i = 0; i < stageData.monsterDatas.Length; i++)
        {
            monsterCards[i].SetCard(stageData.monsterDatas[i].GetCard());
            monsterCards[i].gameObject.SetActive(true);
        }
        //만약 보스몬스터가 존재하면 보스카드 셋팅
        if (GameMgr.Inst.StageData.bossMonsterPrefab)
        {
            bossCard.SetCard(GameMgr.Inst.StageData.bossMonsterPrefab.GetComponent<SetCard>().GetCard());
            bossCard.gameObject.SetActive(true);
        }
        //등장 마리수txt 셋팅
        monsterCountTxt.text = stageData.monsterCount + "마리";
    }

   
    void NextBtn()
    {//다음 버튼 몬스터정보 -> 스킬 카드
        monsterCardPanel.gameObject.SetActive(false);
        skillCardPanel.gameObject.SetActive(true);
        //레벨업이 가능한지 체크후 
        for (int i = 0; i < Skillcards.Length; i++)
        {
            Skillcards[i].gameObject.SetActive(true);

            if (skills[i].skill_Lv ==skills[i].skill_MaxLv)//최대 레벨이면       
                Skillcards[i].gameObject.SetActive(false);  //비활성화     
            else
                Skillcards[i].SetCard(skills[i].GetCard());
        }
        //다음버튼은 필요없으니 비활성화
        nextBtn.gameObject.SetActive(false);
    }

}
