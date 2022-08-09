using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark_Skill : Skill
{
    //주변으로 스파크를 발사함
    public Spark_Collider[] sparkObj;//스파크 오브젝트
    public int[] count; //레벨별 갯수배열 

    public override void Skill_Init()
    {
        for (int i = 0; i < sparkObj.Length; i++) //모든 스파크 콜라이더에 데미지함수 적용
        {
            SkillDamageCollider skillDamageBoxes = sparkObj[i].GetComponentInChildren<SkillDamageCollider>(true);
            skillDamageBoxes.OnTriggerMonster += TakeMonsterDamage;
        }
    }

    public override IEnumerator SkillStart_Co()
    {
        audioSource.Play();
        for (int i = 0; i < count[skill_Lv]; i++)
        {
            //캐릭터 중앙에서  랜덤한 방향으로 발사함       
            sparkObj[i].SetSpark(Random.insideUnitCircle.normalized);
            sparkObj[i].transform.position = hero.transform.position;          
            sparkObj[i].gameObject.SetActive(true);            
        }

        yield return new WaitForSeconds(SkillCool);
        SkillStart();
    }

    public override void SkillRefresh()
    {
        for (int i = 0; i < sparkObj.Length; i++)
            sparkObj[i].gameObject.SetActive(false);

        StopAllCoroutines();
    }
}
