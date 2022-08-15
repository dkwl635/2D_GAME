using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedSword_Skill : Skill
{
    //몬스터를 추적하여 공격하는 칼 소환
    [Header("GuidedSword")]
    public Sprite[] Swordsprites; //레벨별 칼 이미지변경을 위한리스트
    public int[] speed;                //레벨별 속도
    public Transform[] swordPos;    //칼 스폰 위치
    public GameObject[] swordObj;//칼의 오브젝트

    //추적 칼의 정보 변경을위한 리스트
    List<GuidedSword_Collider> guidedSword_Colliders = new List<GuidedSword_Collider>();
    //추적 칼을 발사할때 순서를 정하기 위한
    Queue<GuidedSword_Collider> sword_Qu = new Queue<GuidedSword_Collider>();

    public override void Skill_Init()
    {
       for (int i = 0; i < swordObj.Length; i++) //추적 칼 오브젝트에서 필요한 정보 셋팅
        {
            var skillDamageBoxes = swordObj[i].GetComponentInChildren<GuidedSword_Collider>(true);
            skillDamageBoxes.OnTriggerMonster = TakeMonsterDamage; //데미지함수 적용
            skillDamageBoxes.DeQuObj = this.DequObj; //다시 큐에 들어오기 위한
            skillDamageBoxes.InitSword(swordPos[i].transform); //위치 셋팅
            guidedSword_Colliders.Add(skillDamageBoxes);    //리스트 추가     
            swordObj[i].SetActive(false); //우선 오브젝트 비활성화
        }
    }

    private void Update()
    { //스킬 본체는 캐릭터를 따라가게끔
        transform.position = hero.transform.position;
    }

    public override IEnumerator SkillStart_Co() //스킬 코루틴
    {
        for (int i = 0; i < swordObj.Length; i++)
        {
            swordObj[i].SetActive(true);
            sword_Qu.Enqueue(guidedSword_Colliders[i]); //큐에 넣기
        }
     
        while (true)
        {     
            yield return new WaitForSeconds(SkillCool);

            if (sword_Qu.Count > 0)//대기중인 추적칼이 있다면
            {        
                GameObject targetObj = FindNearestObjectByTag("Monster"); //가까운 몬스터를 찾아 리턴
                if (targetObj != null)
                {
                    GuidedSword_Collider sword = sword_Qu.Dequeue(); //큐에서 빼오기
                    sword.SetTarget(targetObj); //타겟 설정
                    audioSource.Play(); //효과음
                }             
            }
        }
       
    }

    public override void SkillRefresh()
    {
        StopAllCoroutines();
        skill_Co = null;

        //오브젝트 끄기
        for (int i = 0; i < swordObj.Length; i++)
            swordObj[i].SetActive(false);
        
        if (getSkill) //레벨별 추적칼 이미지 적용
            for (int i = 0; i < guidedSword_Colliders.Count; i++)
            {
                if (skill_Lv == 0)
                    guidedSword_Colliders[i].SetSword(Swordsprites[0], speed[0]);
                else
                    guidedSword_Colliders[i].SetSword(Swordsprites[skill_Lv / 2], speed[skill_Lv]);
            }

        sword_Qu.Clear(); //큐 비우기 
    }

    void DequObj(GuidedSword_Collider sword)//다시 큐에 넣어주는
    {
        StartCoroutine(DequObj_Co(sword));
    }

    IEnumerator DequObj_Co(GuidedSword_Collider sword)
    {
        yield return new WaitForSeconds(SkillCool + 0.5f); //잠시 대기하고 넣어줌
        
        if (skill_Co == null)
            yield break;

        sword_Qu.Enqueue(sword);
        sword.transform.gameObject.SetActive(true);
    }

   
}
