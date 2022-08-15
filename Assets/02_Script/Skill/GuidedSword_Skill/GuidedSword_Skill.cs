using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedSword_Skill : Skill
{
    //���͸� �����Ͽ� �����ϴ� Į ��ȯ
    [Header("GuidedSword")]
    public Sprite[] Swordsprites; //������ Į �̹��������� ���Ѹ���Ʈ
    public int[] speed;                //������ �ӵ�
    public Transform[] swordPos;    //Į ���� ��ġ
    public GameObject[] swordObj;//Į�� ������Ʈ

    //���� Į�� ���� ���������� ����Ʈ
    List<GuidedSword_Collider> guidedSword_Colliders = new List<GuidedSword_Collider>();
    //���� Į�� �߻��Ҷ� ������ ���ϱ� ����
    Queue<GuidedSword_Collider> sword_Qu = new Queue<GuidedSword_Collider>();

    public override void Skill_Init()
    {
       for (int i = 0; i < swordObj.Length; i++) //���� Į ������Ʈ���� �ʿ��� ���� ����
        {
            var skillDamageBoxes = swordObj[i].GetComponentInChildren<GuidedSword_Collider>(true);
            skillDamageBoxes.OnTriggerMonster = TakeMonsterDamage; //�������Լ� ����
            skillDamageBoxes.DeQuObj = this.DequObj; //�ٽ� ť�� ������ ����
            skillDamageBoxes.InitSword(swordPos[i].transform); //��ġ ����
            guidedSword_Colliders.Add(skillDamageBoxes);    //����Ʈ �߰�     
            swordObj[i].SetActive(false); //�켱 ������Ʈ ��Ȱ��ȭ
        }
    }

    private void Update()
    { //��ų ��ü�� ĳ���͸� ���󰡰Բ�
        transform.position = hero.transform.position;
    }

    public override IEnumerator SkillStart_Co() //��ų �ڷ�ƾ
    {
        for (int i = 0; i < swordObj.Length; i++)
        {
            swordObj[i].SetActive(true);
            sword_Qu.Enqueue(guidedSword_Colliders[i]); //ť�� �ֱ�
        }
     
        while (true)
        {     
            yield return new WaitForSeconds(SkillCool);

            if (sword_Qu.Count > 0)//������� ����Į�� �ִٸ�
            {        
                GameObject targetObj = FindNearestObjectByTag("Monster"); //����� ���͸� ã�� ����
                if (targetObj != null)
                {
                    GuidedSword_Collider sword = sword_Qu.Dequeue(); //ť���� ������
                    sword.SetTarget(targetObj); //Ÿ�� ����
                    audioSource.Play(); //ȿ����
                }             
            }
        }
       
    }

    public override void SkillRefresh()
    {
        StopAllCoroutines();
        skill_Co = null;

        //������Ʈ ����
        for (int i = 0; i < swordObj.Length; i++)
            swordObj[i].SetActive(false);
        
        if (getSkill) //������ ����Į �̹��� ����
            for (int i = 0; i < guidedSword_Colliders.Count; i++)
            {
                if (skill_Lv == 0)
                    guidedSword_Colliders[i].SetSword(Swordsprites[0], speed[0]);
                else
                    guidedSword_Colliders[i].SetSword(Swordsprites[skill_Lv / 2], speed[skill_Lv]);
            }

        sword_Qu.Clear(); //ť ���� 
    }

    void DequObj(GuidedSword_Collider sword)//�ٽ� ť�� �־��ִ�
    {
        StartCoroutine(DequObj_Co(sword));
    }

    IEnumerator DequObj_Co(GuidedSword_Collider sword)
    {
        yield return new WaitForSeconds(SkillCool + 0.5f); //��� ����ϰ� �־���
        
        if (skill_Co == null)
            yield break;

        sword_Qu.Enqueue(sword);
        sword.transform.gameObject.SetActive(true);
    }

   
}
