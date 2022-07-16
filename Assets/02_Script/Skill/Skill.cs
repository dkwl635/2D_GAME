using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public HeroCtrl hero;
    public int skill_Lv = 1;

    //창 배리어 스킬
    public GameObject[] Lv_Group;

    private void Awake()
    {
        hero = FindObjectOfType<HeroCtrl>();
    }


    private void Start()
    {
        for (int i = 0; i < Lv_Group.Length; i++)
        {
            var skillDamageBoxes = Lv_Group[i].GetComponentsInChildren<SkillDamageColider>(true);
           foreach (var colider in skillDamageBoxes)
           {
                colider.OnTriggerMonster += TakeMonsterDamage;
          }

        }


        SkillStart();
    }
    float timer = 0.0f;
    private void Update()
    {
        timer += Time.deltaTime * 200;

        transform.position = hero.transform.position;
        transform.eulerAngles = new Vector3(0, 0, timer);     
    }

    private Vector3 AngleToDirection(float angle)
    {
        Vector3 direction = hero.transform.position;
        var quaternion = Quaternion.Euler(0, 0, angle);
        Vector3 newDirection = quaternion * direction;

        return newDirection;
    }




    public void SkillStart()
    {
       


    }


    public IEnumerator SkillStart_Co()
    {


        yield return null;
    }


    public void TakeMonsterDamage(Monster monster)
    {
        monster.TakeDamage(10);
    }

}
