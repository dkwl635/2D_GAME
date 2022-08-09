using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteors_Skill : Skill
{
   [Header("Meteor")]
    public GameObject[] meteorObj; //���׿� ������Ʈ��
    public int[] meteors;                  //��ų ������ ����

    public override void Skill_Init()
    {     
        for (int i = 0; i < meteorObj.Length; i++)
        {
            //��ų�ݶ��̴��� ã�� ������ ���������� �Լ� ����//ó������ ������Ʈ ���������� true �� ����
            SkillDamageCollider skillDamageBoxes = meteorObj[i].GetComponentInChildren<SkillDamageCollider>(true);
            skillDamageBoxes.OnTriggerMonster += TakeMonsterDamage;
        }

    }

    public override IEnumerator SkillStart_Co()
    {
        audioSource.Play();

        for (int i = 0; i < meteors[skill_Lv]; i++) //������ ���׿� ����
        {
            //ĳ���� �ֺ� ����ǥ�� ���� ��ȯ
            Vector2 startPos = (Vector2)hero.transform.position + Random.insideUnitCircle * 10;
            meteorObj[i].transform.position = startPos;
            meteorObj[i].SetActive(true);
            yield return new WaitForSeconds(0.1f); //�����ֱ�
        }
        audioSource.Stop();
        //��Ÿ�Ӵ���� �ٽ� ��ų ����
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
