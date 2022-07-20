using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteors_Skill : Skill
{
   [Header("Meteor")]
    public GameObject[] meteorObj;
    public int[] meteors;

    public override void Skill_Init()
    {
      
        for (int i = 0; i < meteorObj.Length; i++)
        {
            SkillDamageCollider skillDamageBoxes = meteorObj[i].GetComponentInChildren<SkillDamageCollider>(true);
            skillDamageBoxes.OnTriggerMonster += TakeMonsterDamage;
        }

    }

    public override IEnumerator SkillStart_Co()
    {
        for (int i = 0; i < meteors[skill_Lv]; i++)
        {
            Vector2 startPos = (Vector2)hero.transform.position + Random.insideUnitCircle * 10;
            meteorObj[i].transform.position = startPos;
            meteorObj[i].SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(5.0f);

        StartCoroutine(SkillStart_Co());
    }

    public override void SkillRefresh()
    {
        StopAllCoroutines();

        for (int i = 0; i < meteorObj.Length; i++)
        {
            meteorObj[i].SetActive(false);
        }
    }

}
