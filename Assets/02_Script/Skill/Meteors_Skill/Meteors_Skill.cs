using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteors_Skill : Skill
{
   [Header("Meteor")]
    public GameObject[] meteorObj; //메테오 오브젝트들
    public int[] meteors;                  //스킬 레벨별 갯수

    public override void Skill_Init()
    {     
        for (int i = 0; i < meteorObj.Length; i++)
        {
            //스킬콜라이더를 찾아 가져와 데미지적용 함수 적용//처음에는 오브젝트 꺼져있으니 true 로 설정
            SkillDamageCollider skillDamageBoxes = meteorObj[i].GetComponentInChildren<SkillDamageCollider>(true);
            skillDamageBoxes.OnTriggerMonster += TakeMonsterDamage;
        }

    }

    public override IEnumerator SkillStart_Co()
    {
        audioSource.Play();

        for (int i = 0; i < meteors[skill_Lv]; i++) //레벨별 메테오 스폰
        {
            //캐릭터 주변 원좌표를 구해 소환
            Vector2 startPos = (Vector2)hero.transform.position + Random.insideUnitCircle * 10;
            meteorObj[i].transform.position = startPos;
            meteorObj[i].SetActive(true);
            yield return new WaitForSeconds(0.1f); //스폰주기
        }
        audioSource.Stop();
        //쿨타임대기후 다시 스킬 시작
        yield return new WaitForSeconds(SkillCool);
        SkillStart();
    }

    public override void SkillRefresh()
    {
        audioSource.Stop();
        StopAllCoroutines();

        for (int i = 0; i < meteorObj.Length; i++)
            meteorObj[i].SetActive(false);
    } 
}
