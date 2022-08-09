using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CardHelp;
using UnityEngine.UI;

public class StageStartCtrl : MonoBehaviour
{//�������� �����ҽ� ��Ÿ���� 
    [Header("UI")]
    public GameObject monsterCardPanel; //������͸� �����ִ�
    public GameObject skillCardPanel;       //��ų �����ϴ� 

    public TextMeshProUGUI monsterCountTxt; //������� ������txt
    public Button nextBtn;                              //�������� �Ѿ��
    [Header("Card_UI")]
    public Card[] monsterCards; //����ī�� 
    public Card bossCard;           //����ī��
    public SkillCard[] Skillcards;  //��ųī��

    StageData stageData;           //���� �������� ����
    Skill[] skills;                         //��ų���

    private void Awake()
    {//��ų ��� ��������
        skills = GameMgr.Inst.skills;
    }

    private void Start()
    {
        nextBtn.onClick.AddListener(NextBtn);

        //��ų �����س���
        for (int i = 0; i < Skillcards.Length; i++)
            Skillcards[i].skill = skills[i];            
    }
    private void OnEnable()
    {//���½�
        //�г�����
        monsterCardPanel.gameObject.SetActive(true);
        skillCardPanel.gameObject.SetActive(false);
        //���� �������� ���� ��������
        stageData = GameMgr.Inst.StageData;
        //��ư Ȱ��ȭ
        nextBtn.gameObject.SetActive(true);
        //������� ī�� �����ϱ�
        for (int i = 0; i < stageData.monsterDatas.Length; i++)
        {
            monsterCards[i].SetCard(stageData.monsterDatas[i].GetCard());
            monsterCards[i].gameObject.SetActive(true);
        }
        //���� �������Ͱ� �����ϸ� ����ī�� ����
        if (GameMgr.Inst.StageData.bossMonsterPrefab)
        {
            bossCard.SetCard(GameMgr.Inst.StageData.bossMonsterPrefab.GetComponent<SetCard>().GetCard());
            bossCard.gameObject.SetActive(true);
        }
        //���� ������txt ����
        monsterCountTxt.text = stageData.monsterCount + "����";
    }

   
    void NextBtn()
    {//���� ��ư �������� -> ��ų ī��
        monsterCardPanel.gameObject.SetActive(false);
        skillCardPanel.gameObject.SetActive(true);
        //�������� �������� üũ�� 
        for (int i = 0; i < Skillcards.Length; i++)
        {
            Skillcards[i].gameObject.SetActive(true);

            if (skills[i].skill_Lv ==skills[i].skill_MaxLv)//�ִ� �����̸�       
                Skillcards[i].gameObject.SetActive(false);  //��Ȱ��ȭ     
            else
                Skillcards[i].SetCard(skills[i].GetCard());
        }
        //������ư�� �ʿ������ ��Ȱ��ȭ
        nextBtn.gameObject.SetActive(false);
    }

}
