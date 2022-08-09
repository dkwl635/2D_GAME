using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CardHelp;

public class LevelUpPanel : MonoBehaviour
{//�������� ��Ÿ�� UI Panel
    public static LevelUpPanel inst;
 
    [Header("LvUpLable")]
    public TextMeshProUGUI lable;
    public Animation lableAnimation;

    [Header("LvUpObj")]
    public GameObject[] lvUpObjs; //�������� ������ ��ų���
    public Ability[] abilities;     //�������� ������ �ɷ�ġ���

    List<ICardLvUp> skillCardList = new List<ICardLvUp>(); //��ųī�� ���
    List<ICardLvUp> abilityCardList = new List<ICardLvUp>(); //�ɷ�ġī�� ���
    List<ICardLvUp> skillLvUpAbleList = new List<ICardLvUp>();//���� �������� ������ ���

     [Header("LvUpCard")]
    public LvUpCard[] lvUpCard; //�������� �����ִ� ī��UI
   
    //Time ����� ����
    float realTimeDalta = 0.0f;
    float animationTime = 0.0f;

    private void Awake()
    {
        inst = this;
        //��� ä���ֱ�
        for (int i = 0; i < lvUpObjs.Length; i++)
            skillCardList.Add(lvUpObjs[i].GetComponent<ICardLvUp>());
       
        for (int i = 0; i < abilities.Length; i++)
            abilityCardList.Add(abilities[i]);
    }

    private void Update()
    {
        //�� ������Ʈ�� ������������   Time.timeScale = 0�̱� ������
        //�ð������ ���־� �ִϸ��̼��� �����ش�.
        float curTime = Time.realtimeSinceStartup;
        float deltaTime = curTime - realTimeDalta;
        realTimeDalta = curTime;

        animationTime += deltaTime;

        LableTxt_Update();
    }

    private void OnEnable()
    {
        CheckLevelPossible(); //�������� ������ �͵��� üũ�Ѵ�.
        SetLvUpCard();          //ī�带 �������ش�.
        Time.timeScale = 0.0f;  //�Ͻ�����
    }


    public void OffPanel()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f; //�ٽ� �ð� ��������
    }

    void LableTxt_Update()
    {
        //���� �ð��� ����Ͽ� �ִϸ��̼��� �����ش�.
        AnimationState a = lableAnimation["LvUpTxt"];
        a.normalizedTime = animationTime % a.length;
    }

    void CheckLevelPossible() //�������� �������� üũ
    {
        skillLvUpAbleList.Clear(); //����Ʈ �ʱ�ȭ
        for (int i = 0; i < skillCardList.Count; i++)
        {
            if (skillCardList[i].LevelPossible() == true) //�������� �������� üũ ��
                skillLvUpAbleList.Add(skillCardList[i]);      //����Ʈ�� �־��ش�.
        }
    }

    void SetLvUpCard() //������ ī�� ����
    {
        int idx = 0; //4���� ī�弱�� ���� 
        List<int> random = new List<int>(); //������ ������ ����
        if(skillLvUpAbleList.Count < 3) //��ų�� 3�� �̸��ϰ��
        {
            //������ �ִ� ��ų ��� ���ð���
            for (int i = 0; i < skillLvUpAbleList.Count; i++) 
                lvUpCard[i].SetCard(skillLvUpAbleList[i]);

            idx += skillLvUpAbleList.Count;
        }
        else //��ų�� 3�� �̻��ϰ��
        {
            while (random.Count <= 2) //2������ �������� ����
            {
                int a = Random.Range(0, skillLvUpAbleList.Count);
                if (random.Contains(a))
                    continue;
                else
                    random.Add(a);
            }
            //������ ������ �´� ��ų�� ����
            for (int i = 0; i < random.Count; i++)
            {
                lvUpCard[i].SetCard(skillLvUpAbleList[random[i]]);
                idx++;
            }
        }
        //��ų���� �� �ɷ�ġ ����
        random.Clear(); 
        while (random.Count < 4 - idx) //�ߺ����� �ʰ� ����
        {
            int a = Random.Range(0, abilityCardList.Count);
            if (random.Contains(a))
                continue;
            else
                random.Add(a);
        }
   
        for (int i = 0; i < random.Count; i++)  //������ ������ �´� �ɷ�ġ�� ����
            lvUpCard[idx + i].SetCard(abilityCardList[random[i]]);
        
    }
}
