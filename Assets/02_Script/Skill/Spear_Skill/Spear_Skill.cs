using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_Skill : Skill
{
    //�ֺ��� ���ƴٴϸ� �����ϴ� â ��ȯ 
    [Header("Spear")]
    public GameObject[] Lv_Group; //������ â �׷�
    //������ ���� �׷� ������Ʈ ����
    GameObject CurSkillObj { get { if (skill_Lv == 0) return Lv_Group[0]; else return Lv_Group[skill_Lv / 2]; } }

    public override void Skill_Init()
    {  
        for (int i = 0; i < Lv_Group.Length; i++)
        {
            //SkillDamageCollider �� �������� �ִ� �Լ��� �����Ѵ�.
            var skillDamageBoxes = Lv_Group[i].GetComponentsInChildren<SkillDamageCollider>(true);
            foreach (var colider in skillDamageBoxes)
                colider.OnTriggerMonster += TakeMonsterDamage;
        }
    }

    public override IEnumerator SkillStart_Co()
    {
        float timer = 5.0f; //5�ʵ��� 
        float angle = 0.0f; //ȸ�� ����

        CurSkillObj.SetActive(true); //������ ������Ʈ on
        audioSource.Play();//ȿ����
        while(timer >  0.0f) 
        {           
            timer -= Time.deltaTime; 
            angle += Time.deltaTime * 200;
            //��ġ�� ȸ�� ����
            transform.position = hero.transform.position;
            transform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }
        //���ӽð� ������ 
        audioSource.Stop();
        CurSkillObj.SetActive(false);

        //��ų ��Ÿ�� ���� �ٽ� ��ŸƮ
        yield return new WaitForSeconds(SkillCool);
        SkillStart();
     
    }

    public override void SkillRefresh()
    {
        audioSource.Stop();
        //��� ������Ʈ ����
        for (int i = 0; i < Lv_Group.Length; i++)
            Lv_Group[i].SetActive(false);
        //�������� �ڷ�ƾ ����
        StopAllCoroutines();
    }

}
