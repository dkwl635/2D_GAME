using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuidedSword_Skill : Skill
{
    [Header("GuidedSword")]
    public Transform[] swordPos;
    public GameObject[] swordObj;

    Queue<GuidedSword_Collider> sword_Qu = new Queue<GuidedSword_Collider>();



    public override void Skill_Init()
    {
        skillLvInfo[0] = "주변에 메테오 공격";
        skillLvInfo[1] = "공격력 증가";
        skillLvInfo[2] = "메테오의 갯수 증가";
        skillLvInfo[3] = "공격력 증가";
        skillLvInfo[4] = "메테오의 갯수 증가";
        skillLvInfo[5] = "공격력 증가";
        skillLvInfo[6] = "메테오의 갯수 증가";

        for (int i = 0; i < swordObj.Length; i++)
        {
            var skillDamageBoxes = swordObj[i].GetComponentInChildren<GuidedSword_Collider>(true);
            skillDamageBoxes.OnTriggerMonster = TakeMonsterDamage;
            skillDamageBoxes.DeQuObj = this.DequObj;
            skillDamageBoxes.SetSword(swordPos[i].transform);
            sword_Qu.Enqueue(skillDamageBoxes);

            swordObj[i].transform.position = swordPos[i].position;
            swordObj[i].SetActive(false);
        }
    }

    private void Update()
    {
        transform.position = hero.transform.position;
    }

    public override IEnumerator SkillStart_Co()
    {
        SkillRefresh();

        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            if (sword_Qu.Count > 0)
            {
                GuidedSword_Collider sword = sword_Qu.Dequeue();
                GameObject targetObj = FindNearestObjectByTag("Monster");
                if (targetObj != null)
                {
                    sword.SetTarget(targetObj.GetComponent<Monster>());
                }
            }
        }
       
    }

    public override void SkillRefresh()
    {
        if (getSkill)
        {
            for (int i = 0; i < swordObj.Length; i++)
            {
                swordObj[i].transform.position = swordPos[i].position;
                swordObj[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < swordObj.Length; i++)
            {
                swordObj[i].SetActive(false);
            }
        }
    }

    void DequObj(GuidedSword_Collider sword)
    {
        
        StartCoroutine(DequObj_Co(sword));
    }

    IEnumerator DequObj_Co(GuidedSword_Collider sword)
    {
        yield return new WaitForSeconds(1.0f);
        sword_Qu.Enqueue(sword);
        sword.transform.gameObject.SetActive(true);
    }

    private GameObject FindNearestObjectByTag(string tag)//가장 가까운 유닛찾기
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = objects
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault(); //첫번째 요서 반환 없으면 null;

        return neareastObject;
    }
}
