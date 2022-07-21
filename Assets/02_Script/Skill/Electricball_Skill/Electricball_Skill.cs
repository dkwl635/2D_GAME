using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricball_Skill : Skill
{
    public Electricball_Collider electricball;
    public float speed = 1.0f;

    public override void Skill_Init()
    {

            electricball.OnTriggerMonster += TakeMonsterDamage;
    }

    private void Update()
    {
        transform.position = hero.transform.position;
    }

    public override IEnumerator SkillStart_Co()
    {
        electricball.gameObject.SetActive(true);

        if (skill_Lv == 0)
            electricball.transform.localScale = Vector3.one;
        else
            electricball.transform.localScale = Vector3.one * (1 + skill_Lv / 2);

        while (true)
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
