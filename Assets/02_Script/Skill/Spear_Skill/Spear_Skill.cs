using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_Skill : Skill
{
    //주변을 돌아다니면 공격하는 창 소환 
    [Header("Spear")]
    public GameObject[] Lv_Group; //레벨별 창 그룹
    //레벨에 따른 그룹 오브젝트 리턴
    GameObject CurSkillObj { get { if (skill_Lv == 0) return Lv_Group[0]; else return Lv_Group[skill_Lv / 2]; } }

    public override void Skill_Init()
    {  
        for (int i = 0; i < Lv_Group.Length; i++)
        {
            //SkillDamageCollider 에 데미지를 주는 함수를 적용한다.
            var skillDamageBoxes = Lv_Group[i].GetComponentsInChildren<SkillDamageCollider>(true);
            foreach (var colider in skillDamageBoxes)
                colider.OnTriggerMonster += TakeMonsterDamage;
        }
    }

    public override IEnumerator SkillStart_Co()
    {
        float timer = 5.0f; //5초동안 
        float angle = 0.0f; //회전 각도

        CurSkillObj.SetActive(true); //레벨별 오브젝트 on
        audioSource.Play();//효과음
        while(timer >  0.0f) 
        {           
            timer -= Time.deltaTime; 
            angle += Time.deltaTime * 200;
            //위치와 회전 조정
            transform.position = hero.transform.position;
            transform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }
        //지속시간 끝나고 
        audioSource.Stop();
        CurSkillObj.SetActive(false);

        //스킬 쿨타임 이후 다시 스타트
        yield return new WaitForSeconds(SkillCool);
        SkillStart();
     
    }

    public override void SkillRefresh()
    {
        audioSource.Stop();
        //모든 오브젝트 끄기
        for (int i = 0; i < Lv_Group.Length; i++)
            Lv_Group[i].SetActive(false);
        //실행중인 코루틴 중지
        StopAllCoroutines();
    }

}
