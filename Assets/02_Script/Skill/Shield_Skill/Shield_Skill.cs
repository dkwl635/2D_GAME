using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Skill : Skill
{
    //power == cooltime
    [Header("Shield")]
    public Shield_Collider Shield;


    public override IEnumerator SkillStart_Co()
    {
        while(true)
        {
            yield return new WaitForSeconds(skillPw[skill_Lv]);

            GameObject monsterObj = FindNearestObjectByTag("Monster");
            if(monsterObj)
            {
                Vector2 temp = monsterObj.transform.position - hero.transform.position;
             
                if (temp.magnitude < 5.0f)
                {
                    Shield.SpawnShield(hero.transform.position, temp.normalized);
                }

                yield return new WaitForSeconds(0.5f);
                Shield.gameObject.SetActive(false);
            }

        }
    }

    public override void SkillRefresh()
    {
        Shield.gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
