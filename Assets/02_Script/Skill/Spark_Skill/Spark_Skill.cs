using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark_Skill : Skill
{
    //�ֺ����� ����ũ�� �߻���
    public Spark_Collider[] sparkObj;//����ũ ������Ʈ
    public int[] count; //������ �����迭 

    public override void Skill_Init()
    {
        for (int i = 0; i < sparkObj.Length; i++) //��� ����ũ �ݶ��̴��� �������Լ� ����
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
            //ĳ���� �߾ӿ���  ������ �������� �߻���       
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
