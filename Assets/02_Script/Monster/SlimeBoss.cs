using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;
public class SlimeBoss : BossMonster
{ //슬라임 보스 몬스터
    [SerializeField] float jumpDis; //점프 공격거리 조건
    [SerializeField] GameObject shawdow; //점프 공격시 그림자Obj
    [SerializeField] BossMonsterDamageCollider atkCollider; //보스몬스터 공격 콜라이더

    protected override void Start()
    {
        base.Start();
        //보스몬스터 공격 콜라이더 함수 적용
        atkCollider.TakeDamge += () => { targetHero.TakeDamage(attackPower); };
    }

    protected void FixedUpdate()
    {
        //이동 함수
        if (monster_State == Monster_State.Move || monster_State == Monster_State.Attack1)
            rigidbody.MovePosition(transform.position + targetToThis.normalized * speed * Time.fixedDeltaTime);
    }

    protected override void Update()
    {
        base.Update();
        
        if (monster_State == Monster_State.Die || monster_State == Monster_State.Attack1 || monster_State == Monster_State.Attack2)
            return;
        //거리에 따른 상태 변환
        if (dis <= attakcDis)
            MonsterState_Update(Monster_State.Attack1); //근접공격
        else if (attakcDis + 100.0f  < dis &&dis < jumpDis)
            MonsterState_Update(Monster_State.Attack2);//점프공격
        else 
            MonsterState_Update(Monster_State.Move);
    }


    protected override void MonsterState_Update(Monster_State newStatue) //몬스터의 상태 변화에 따른 애니메이터 변환해주기
    {
        if (monster_State.Equals(newStatue)) return; //이전과 같은 상태면 리턴

        if (monster_State.Equals(Monster_State.Die)) return; //죽은 상태면

        //새로운 상태 적용
        monster_State = newStatue;
        //속도 초기화
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
    {//애니메이션에 들어가는 함수
        //공격 콜라이더 활성화
        atkCollider.gameObject.SetActive(true);
    }

    public void Atk01End_Event()
    {//애니메이션에 들어가는 함수
        //근접 공격 종료
        atkCollider.gameObject.SetActive(false);
        MonsterState_Update(Monster_State.Idle);
    }
    
    public void Atk02Start_Event()
    {//애니메이션에 들어가는 함수
        //점프 공격
        StartCoroutine(Jump());  
    }
    public void Atk02End_Event()
    {  //애니메이션에 들어가는 함수
        shawdow.SetActive(false);
        atkCollider.gameObject.SetActive(false);
        StopAllCoroutines();
        MonsterState_Update(Monster_State.Idle);
    }

    IEnumerator Jump()
    {
        //점프 위치 잡기
        Vector3 startPos = transform.position;
        Vector3 midPos = startPos;
        midPos.y += 40.0f;
        Vector3 endPos = targetTr.position;
        
        shawdow.SetActive(true);
        while (true) //점프처럼 보이게 하기 위한 위치
        {       
            transform.position = Jump(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, startPos , midPos, endPos);
            shawdow.transform.position = endPos + new Vector3(0, -1.5f);
            yield return null;
        }

        atkCollider.gameObject.SetActive(true);
    }

    public Vector2 Jump(float timer, Vector2 p1, Vector2 p2, Vector2 p3)//곡선 점프처럼 위치값 넣어주기
    {
        Vector2 a = Vector3.Lerp(p1, p2, timer);
        Vector2 b = Vector3.Lerp(p2, p3, timer);

        return Vector2.Lerp(a, b, timer) + Vector2.up;
    }
}
