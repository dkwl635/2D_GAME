using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public HeroCtrl hero;
    public int skill_Lv = 0;
    public int skill_MaxLv = 0;

    string[] skillLvInfo = new string[7];

    public int[] skillPw = { 1,1, 2, 2, 4, 4, 6, 6 };
    public string SkillInfo
    {
        get { return skillLvInfo[skill_Lv]; }
    }

    public Sprite skillSprite;

    //â ȸ����ų
    public GameObject[] Lv_Group;
    GameObject CurSkillObj { get { if (skill_Lv <= 2)return Lv_Group[0]; else return Lv_Group[skill_Lv / 2]; } }

    private void Awake()
    {
        hero = FindObjectOfType<HeroCtrl>();

        skillLvInfo[0] = "�ֺ��� ���ƴٴϴ� â�� ��ȯ";  
        skillLvInfo[1] = "���ݷ� ����";          
        skillLvInfo[2] = "â�� ���� ����";    
        skillLvInfo[3] = "���ݷ� ����";
        skillLvInfo[4] = "â�� ���� ����";
        skillLvInfo[5] = "���ݷ� ����";
        skillLvInfo[6] = "â�� ���� ����";
    }


    private void Start()
    {
        for (int i = 0; i < Lv_Group.Length; i++)
        {
            var skillDamageBoxes = Lv_Group[i].GetComponentsInChildren<SkillDamageColider>(true);
           foreach (var colider in skillDamageBoxes) 
                colider.OnTriggerMonster += TakeMonsterDamage;
        }

    }

    private void Update()
    {
        
    }

    private Vector3 AngleToDirection(float angle)
    {
        Vector3 direction = hero.transform.position;
        var quaternion = Quaternion.Euler(0, 0, angle);
        Vector3 newDirection = quaternion * direction;

        return newDirection;
    }




    public void SkillStart()
    {
       // StopAllCoroutines();
        StartCoroutine(SkillStart_Co());
    }


    public IEnumerator SkillStart_Co()
    {
        float timer = 0.0f;
        float speed = 0.0f;

        CurSkillObj.SetActive(true);

        Debug.Log(skill_Lv + " : " + skill_Lv/2);
        while (timer < 5.0f)
        {
            yield return null;
            timer += Time.deltaTime;
            speed += Time.deltaTime * 200;
            transform.position = hero.transform.position;
            transform.eulerAngles = new Vector3(0, 0, speed);

        }
        CurSkillObj.SetActive(false);

        yield return new WaitForSeconds(3.0f);
        StartCoroutine(SkillStart_Co());


    }

    public void SkillLvUp()
    {
        StopAllCoroutines();
        CurSkillObj.SetActive(false);
        skill_Lv++;

        SkillStart();
    }

    public int SkillDamage()
    {
        return skillPw[skill_Lv] + hero.skillPower;
    }

    public void TakeMonsterDamage(Monster monster)
    {
        monster.TakeDamage(SkillDamage());
    }

}
