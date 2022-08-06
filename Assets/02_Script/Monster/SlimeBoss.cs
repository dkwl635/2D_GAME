using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;
public class SlimeBoss : BossMonster
{ //������ ���� ����
    [SerializeField] float jumpDis; //���� ���ݰŸ� ����
    [SerializeField] GameObject shawdow; //���� ���ݽ� �׸���Obj
    [SerializeField] BossMonsterDamageCollider atkCollider; //�������� ���� �ݶ��̴�

    protected override void Start()
    {
        base.Start();
        //�������� ���� �ݶ��̴� �Լ� ����
        atkCollider.TakeDamge += () => { targetHero.TakeDamage(attackPower); };
    }

    protected void FixedUpdate()
    {
        //�̵� �Լ�
        if (monster_State == Monster_State.Move || monster_State == Monster_State.Attack1)
            rigidbody.MovePosition(transform.position + targetToThis.normalized * speed * Time.fixedDeltaTime);
    }

    protected override void Update()
    {
        base.Update();
        
        if (monster_State == Monster_State.Die || monster_State == Monster_State.Attack1 || monster_State == Monster_State.Attack2)
            return;
        //�Ÿ��� ���� ���� ��ȯ
        if (dis <= attakcDis)
            MonsterState_Update(Monster_State.Attack1); //��������
        else if (attakcDis + 100.0f  < dis &&dis < jumpDis)
            MonsterState_Update(Monster_State.Attack2);//��������
        else 
            MonsterState_Update(Monster_State.Move);
    }


    protected override void MonsterState_Update(Monster_State newStatue) //������ ���� ��ȭ�� ���� �ִϸ����� ��ȯ���ֱ�
    {
        if (monster_State.Equals(newStatue)) return; //������ ���� ���¸� ����

        if (monster_State.Equals(Monster_State.Die)) return; //���� ���¸�

        //���ο� ���� ����
        monster_State = newStatue;
        //�ӵ� �ʱ�ȭ
        rigidbody.velocity = Vector2.zero;

        switch (monster_State)
        {
            case Monster_State.Idle:
                animator.SetBool("Move", false);

                break;
            case Monster_State.Move:
                animator.SetBool("Move", true);
                break;
            case Monster_State.Attack1:
                {
                    animator.SetBool("Move", false);
                    animator.SetTrigger("Attack01");
                }
                break;
            case Monster_State.Attack2:
                {
                    animator.SetBool("Move", false);                  
                    animator.SetTrigger("Attack02");
                }
                break;
            case Monster_State.Die:
                {
                    animator.speed = 0.0f;
                    StartCoroutine(Die_Co());
                }
                break;
            default:
                break;
        }


    }

    void Atk01Damage_Event()
    {//�ִϸ��̼ǿ� ���� �Լ�
        //���� �ݶ��̴� Ȱ��ȭ
        atkCollider.gameObject.SetActive(true);
    }

    public void Atk01End_Event()
    {//�ִϸ��̼ǿ� ���� �Լ�
        //���� ���� ����
        atkCollider.gameObject.SetActive(false);
        MonsterState_Update(Monster_State.Idle);
    }
    
    public void Atk02Start_Event()
    {//�ִϸ��̼ǿ� ���� �Լ�
        //���� ����
        StartCoroutine(Jump());  
    }
    public void Atk02End_Event()
    {  //�ִϸ��̼ǿ� ���� �Լ�
        shawdow.SetActive(false);
        atkCollider.gameObject.SetActive(false);
        StopAllCoroutines();
        MonsterState_Update(Monster_State.Idle);
    }

    IEnumerator Jump()
    {
        //���� ��ġ ���
        Vector3 startPos = transform.position;
        Vector3 midPos = startPos;
        midPos.y += 40.0f;
        Vector3 endPos = targetTr.position;
        
        shawdow.SetActive(true);
        while (true) //����ó�� ���̰� �ϱ� ���� ��ġ
        {       
            transform.position = Jump(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, startPos , midPos, endPos);
            shawdow.transform.position = endPos + new Vector3(0, -1.5f);
            yield return null;
        }

        atkCollider.gameObject.SetActive(true);
    }

    public Vector2 Jump(float timer, Vector2 p1, Vector2 p2, Vector2 p3)//� ����ó�� ��ġ�� �־��ֱ�
    {
        Vector2 a = Vector3.Lerp(p1, p2, timer);
        Vector2 b = Vector3.Lerp(p2, p3, timer);

        return Vector2.Lerp(a, b, timer) + Vector2.up;
    }
}
