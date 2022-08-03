using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss : BossMonster
{
    protected override void Start()
    {
        base.Start();

    }

    protected void FixedUpdate()
    {
        if (monster_State == Monster_State.Move || monster_State == Monster_State.Attack1)
            rigidbody.MovePosition(transform.position + targetToThis.normalized * speed * Time.fixedDeltaTime);
    }
    protected override void Update()
    {
        base.Update();

        if (monster_State == Monster_State.Die || monster_State == Monster_State.Attack1 || monster_State == Monster_State.Attack2)
            return;
        //���̿� ���� ���� ��ȯ

        if (dis <= attakcDis)
            MonsterState_Update(Monster_State.Attack1);
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
}
