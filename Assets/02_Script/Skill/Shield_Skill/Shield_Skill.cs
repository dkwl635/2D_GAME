using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Skill : Skill
{
    //가까운 적이 있으면 밀쳐주는 방패스킬

    [Header("Shield")]
    public Shield_Collider Shield; //방패 오브젝트

    //방패 스킬은 스킬데미지가 곧 쿨타임이 된다

    public override IEnumerator SkillStart_Co()
    {
        while(true)
        {        
            GameObject monsterObj = FindNearestObjectByTag("Monster"); //가까운 몬스터를 찾아 리턴
            if(monsterObj)
            {
                Vector2 temp = monsterObj.transform.position - hero.transform.position;             
                if (temp.magnitude < 5.0f) //거리 체크
                {
                    audioSource.Play();
                    //밀치는방패 소환
                    Shield.SpawnShield(hero.transform.position, temp.normalized);
                }
                //0.5초후 사라지게
                yield return new WaitForSeconds(0.5f);
                Shield.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(skillPw[skill_Lv]);
        }
    }

    public override void SkillRefresh()
    {
        Shield.gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
