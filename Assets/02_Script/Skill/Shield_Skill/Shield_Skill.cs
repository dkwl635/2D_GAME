using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Skill : Skill
{
    //����� ���� ������ �����ִ� ���н�ų

    [Header("Shield")]
    public Shield_Collider Shield; //���� ������Ʈ

    //���� ��ų�� ��ų�������� �� ��Ÿ���� �ȴ�

    public override IEnumerator SkillStart_Co()
    {
        while(true)
        {        
            GameObject monsterObj = FindNearestObjectByTag("Monster"); //����� ���͸� ã�� ����
            if(monsterObj)
            {
                Vector2 temp = monsterObj.transform.position - hero.transform.position;             
                if (temp.magnitude < 5.0f) //�Ÿ� üũ
                {
                    audioSource.Play();
                    //��ġ�¹��� ��ȯ
                    Shield.SpawnShield(hero.transform.position, temp.normalized);
                }
                //0.5���� �������
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
