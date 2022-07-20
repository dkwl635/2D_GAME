using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark_Skill : Skill
{
    public Spark_Collider[] sparkObj;
    public int[] count;

    public override void Skill_Init()
    {
        for (int i = 0; i < sparkObj.Length; i++)
        {
            SkillDamageCollider skillDamageBoxes = sparkObj[i].GetComponentInChildren<SkillDamageCollider>(true);
            skillDamageBoxes.OnTriggerMonster += TakeMonsterDamage;
        }
    }

    public override IEnumerator SkillStart_Co()
    {
        for (int i = 0; i < count[skill_Lv]; i++)
        {
            Vector2 dir =  Random.insideUnitCircle;
            sparkObj[i].transform.position = hero.transform.position;
            sparkObj[i].SetSpark(dir);
            sparkObj[i].gameObject.SetActive(true);            
        }

        yield return new WaitForSeconds(5.0f);

        StartCoroutine(SkillStart_Co());
    }

    public override void SkillRefresh()
    {
        for (int i = 0; i < sparkObj.Length; i++)
            sparkObj[i].gameObject.SetActive(false);

        StopAllCoroutines();
    }
}
