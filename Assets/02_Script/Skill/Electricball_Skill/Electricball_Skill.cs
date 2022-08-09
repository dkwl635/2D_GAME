using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricball_Skill : Skill
{
    //캐릭터 주변을 돌아다니며 전기구체 소환
    public Electricball_Collider electricball;
    public float speed = 1.0f;

    public override void Skill_Init()
    {
        //전기구체 콜라이더에 데미지함수 적용
      electricball.OnTriggerMonster += TakeMonsterDamage;
    }

    private void Update()
    {
        //스킬 본체 위치조정
        transform.position = hero.transform.position;
    }

    public override IEnumerator SkillStart_Co()
    {
        electricball.gameObject.SetActive(true);

        //구체는 레벨별로 크기가 커지게
        if (skill_Lv == 0)
            electricball.transform.localScale = Vector3.one;
        else
            electricball.transform.localScale = Vector3.one * (1 + skill_Lv / 2);

        while (true)//주변을 게속돌아감
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
