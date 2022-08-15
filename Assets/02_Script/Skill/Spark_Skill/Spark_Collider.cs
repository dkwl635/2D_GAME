using MonsterHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark_Collider : SkillDamageCollider
{
    Vector2 dir; //뱡향
    Animator animator; //애니메이션 변경을 위한
    float lifeTime = 3.0f; //생존시간
    float speed = 10.0f; //날아가는 속도

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    private void OnEnable() //스파크 발사
    {
        collider2D.enabled = true; //콜라이더 on
        speed = 10.0f;                  //속도 
        lifeTime = 3.0f;                //3초후 제거
    }
    public void SetSpark(Vector2 dir)
    {
        this.dir = dir;
        transform.right = dir; //뱡향에따른 회전
        gameObject.SetActive(true);
    }

    private void Update()
    {      
        //움직이기
        transform.position += (Vector3)dir * Time.deltaTime * speed;
        
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")) //충돌시
        {
            //데미지 적용후
            OnTriggerMonster?.Invoke(collision.GetComponent<ITakeDamage>());
            //콜라이더 off
            collider2D.enabled = false;
            //애니메이션 히트 판정애니메이션 실행
            animator.SetTrigger("Hit");
            speed = 0.0f;
            lifeTime = 0.5f;
        }
    }

}
