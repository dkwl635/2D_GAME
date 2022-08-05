using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedSword_Skill : Skill
{
    [Header("GuidedSword")]
    public Sprite[] Swordsprites;
    public int[] speed;
    public Transform[] swordPos;
    public GameObject[] swordObj;

    List<GuidedSword_Collider> guidedSword_Colliders = new List<GuidedSword_Collider>();
    Queue<GuidedSword_Collider> sword_Qu = new Queue<GuidedSword_Collider>();

  

    public override void Skill_Init()
    {
       for (int i = 0; i < swordObj.Length; i++)
        {
            var skillDamageBoxes = swordObj[i].GetComponentInChildren<GuidedSword_Collider>(true);
            skillDamageBoxes.OnTriggerMonster = TakeMonsterDamage;
            skillDamageBoxes.DeQuObj = this.DequObj;
            skillDamageBoxes.InitSword(swordPos[i].transform);
            guidedSword_Colliders.Add(skillDamageBoxes);
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
        for (int i = 0; i < swordObj.Length; i++)
        {
            swordObj[i].transform.position = swordPos[i].position;
            swordObj[i].SetActive(true);
            sword_Qu.Enqueue(guidedSword_Colliders[i]);
        }

       
        while (true)
        {     
            yield return new WaitForSeconds(SkillCool);

            if (sword_Qu.Count > 0)
            {        
                GameObject targetObj = FindNearestObjectByTag("Monster");
                if (targetObj != null)
                {
                    GuidedSword_Collider sword = sword_Qu.Dequeue();
                    sword.SetTarget(targetObj);
                    audioSource.Play();
                }             
            }
        }
       
    }

    public override void SkillRefresh()
    {
        for (int i = 0; i < swordObj.Length; i++)
        {
            swordObj[i].SetActive(false);
        }

        if (getSkill)
            for (int i = 0; i < guidedSword_Colliders.Count; i++)
            {
                if (skill_Lv == 0)
                    guidedSword_Colliders[i].SetSword(Swordsprites[0], speed[0]);
                else
                    guidedSword_Colliders[i].SetSword(Swordsprites[skill_Lv / 2], speed[skill_Lv]);
            }

        sword_Qu.Clear();

        StopAllCoroutines();
    }

    void DequObj(GuidedSword_Collider sword)
    {
        StartCoroutine(DequObj_Co(sword));
    }

    IEnumerator DequObj_Co(GuidedSword_Collider sword)
    {
        yield return new WaitForSeconds(SkillCool);
     
        sword_Qu.Enqueue(sword);
        sword.transform.gameObject.SetActive(true);
    }

   
}
