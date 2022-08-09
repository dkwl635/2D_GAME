using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricball_Skill : Skill
{
    //ĳ���� �ֺ��� ���ƴٴϸ� ���ⱸü ��ȯ
    public Electricball_Collider electricball;
    public float speed = 1.0f;

    public override void Skill_Init()
    {
        //���ⱸü �ݶ��̴��� �������Լ� ����
      electricball.OnTriggerMonster += TakeMonsterDamage;
    }

    private void Update()
    {
        //��ų ��ü ��ġ����
        transform.position = hero.transform.position;
    }

    public override IEnumerator SkillStart_Co()
    {
        electricball.gameObject.SetActive(true);

        //��ü�� �������� ũ�Ⱑ Ŀ����
        if (skill_Lv == 0)
            electricball.transform.localScale = Vector3.one;
        else
            electricball.transform.localScale = Vector3.one * (1 + skill_Lv / 2);

        while (true)//�ֺ��� �Լӵ��ư�
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);
            yield return null;
        }
    }

    public override void SkillRefresh()
    {
        electricball.gameObject.SetActive(false);
        StopAllCoroutines();
    }

}
