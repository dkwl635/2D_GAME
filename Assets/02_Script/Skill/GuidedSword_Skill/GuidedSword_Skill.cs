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
        skillLvInfo[0] = "�ֺ��� ���׿� ����";
        skillLvInfo[1] = "���ݷ� ����";
        skillLvInfo[2] = "���׿��� ���� ����";
        skillLvInfo[3] = "���ݷ� ����";
        skillLvInfo[4] = "���׿��� ���� ����";
        skillLvInfo[5] = "���ݷ� ����";
        skillLvInfo[6] = "���׿��� ���� ����";

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

    private GameObject FindNearestObjectByTag(string tag)//���� ����� ����ã��
    {
        // Ž���� ������Ʈ ����� List �� �����մϴ�.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ �޼ҵ带 �̿��� ���� ����� ���� ã���ϴ�.
        var neareastObject = objects
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault(); //ù��° �伭 ��ȯ ������ null;

        return neareastObject;
    }
}
