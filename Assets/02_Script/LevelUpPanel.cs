using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpPanel : MonoBehaviour
{
    HeroCtrl hero;
    public TextMeshProUGUI lable;
    public Animation lableAnimation;


    public Skill skill;
    public Transform skillGroup;
    public GameObject skillLvUpBtnObj;

    public SkillLvUpBtn[] skillLvUpBtns = new SkillLvUpBtn[3];

    bool gameStart = true;

    private void Awake()
    {
       
    }

    private void Start()
    {
        hero = GameMgr.Inst.hero;            
    }

    private void OnEnable()
    {
        if (gameStart)
        {
            lable.text = "Game Start";
            gameStart = false;
        }
        else
            lable.text = "Level Up!!";

        skillLvUpBtns[0].SetBtn(skill, OffPanel);
    }

    float realTimeDalta = 0.0f;
    float animationTime = 0.0f;
    private void Update()
    {
        float curTime = Time.realtimeSinceStartup;
        float deltaTime = curTime - realTimeDalta;
        realTimeDalta = curTime;

        animationTime += deltaTime;


        LableTxt_Update();
    }


    public void OffPanel()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;

        realTimeDalta = 0.0f;
        animationTime = 0.0f;
    }

    void LableTxt_Update()
    {
        AnimationState a = lableAnimation["LvUpTxt"];  
        a.normalizedTime = animationTime % a.length;
    }

}
