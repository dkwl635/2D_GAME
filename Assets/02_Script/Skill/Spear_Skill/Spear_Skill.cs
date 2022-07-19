using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_Skill : Skill
{
    //창 회전스킬
    public GameObject[] Lv_Group;
    GameObject CurSkillObj { get { if (skill_Lv == 0) return Lv_Group[0]; else return Lv_Group[skill_Lv / 2]; } }


    public override void Skill_Init()
    {
        skillLvInfo[0] = "주변을 돌아다니는 창을 소환";
        skillLvInfo[1] = "공격력 증가";
        skillLvInfo[2] = "창의 갯수 증가";
        skillLvInfo[3] = "공격력 증가";
        skillLvInfo[4] = "창의 갯수 증가";
        skillLvInfo[5] = "공격력 증가";
        skillLvInfo[6] = "창의 갯수 증가";

        for (int i = 0; i < Lv_Group.Length; i++)
        {
            var skillDamageBoxes = Lv_Group[i].GetComponentsInChildren<SkillDamageCollider>(true);
            foreach (var colider in skillDamageBoxes)
                colider.OnTriggerMonster += TakeMonsterDamage;
        }
    }



    public override IEnumerator SkillStart_Co()
    {
        float timer = 0.0f;
        float speed = 0.0f;

        CurSkillObj.SetActive(true);
        while(timer <  5.0f)
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

    public override void SkillRefresh()
    {
        CurSkillObj.SetActive(false);
    }


    private Vector3 AngleToDirection(float angle)
    {
        Vector3 direction = hero.transform.position;
        var quaternion = Quaternion.Euler(0, 0, angle);
        Vector3 newDirection = quaternion * direction;

        return newDirection;
    }



}
