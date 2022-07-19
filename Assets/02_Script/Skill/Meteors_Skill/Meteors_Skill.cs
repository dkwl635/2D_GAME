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
        skillLvInfo[0] = "주변에 메테오 공격";
        skillLvInfo[1] = "공격력 증가";
        skillLvInfo[2] = "메테오의 갯수 증가";
        skillLvInfo[3] = "공격력 증가";
        skillLvInfo[4] = "메테오의 갯수 증가";
        skillLvInfo[5] = "공격력 증가";
        skillLvInfo[6] = "메테오의 갯수 증가";

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
   

}
