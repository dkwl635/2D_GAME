using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_Skill : Skill
{
    [Header("Spear")]
    //창 회전스킬
    public GameObject[] Lv_Group;
    GameObject CurSkillObj { get { if (skill_Lv == 0) return Lv_Group[0]; else return Lv_Group[skill_Lv / 2]; } }


    public override void Skill_Init()
    {  
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
            timer += Time.deltaTime;
            speed += Time.deltaTime * 200;
            transform.position = hero.transform.position;
            transform.eulerAngles = new Vector3(0, 0, speed);

            yield return null;
        }
        CurSkillObj.SetActive(false);

        yield return new WaitForSeconds(SkillCool);
        Debug.Log("SkillCool");
        StartCoroutine(SkillStart_Co());
     
    }

    public override void SkillRefresh()
    {
        Debug.Log("Refresh");

        for (int i = 0; i < Lv_Group.Length; i++)
            Lv_Group[i].SetActive(false);
      
        StopAllCoroutines();
    }


    private Vector3 AngleToDirection(float angle)
    {
        Vector3 direction = hero.transform.position;
        var quaternion = Quaternion.Euler(0, 0, angle);
        Vector3 newDirection = quaternion * direction;

        return newDirection;
    }



}
