using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;

public class Meteors_Collider : SkillDamageCollider
{
   public AudioSource audioSource; 
    Vector2 dir = new Vector2(1.0f, -1.0f).normalized; //대각선 방향
    bool move = false;   //움직이는 지
    private void Update()
    {
        if (move)//이동 가능하면 대각선으로 떨어지는 것처럼 이동
            transform.position += (Vector3)dir * Time.deltaTime * 10.0f;
    }

    private void OnEnable()
    {
        //콜라이더를 끄고 이동
        collider2D.enabled = false;
        move = true;
    }

    public void Meteors_Evenet()
    {//메테오 애니메이션에 맞추어 터지는 순간
        //콜라이더를 키고 움직임을 막는다.
        audioSource.Play();
        collider2D.enabled = true;
        move = false;
    }
   public void MeteorsEnd_Evenet()
    {//애니메이션 종료시 오브젝트 끄기
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")) //몬스터에게 데미지 판정
            OnTriggerMonster?.Invoke(collision.GetComponent<ITakeDamage>());
    }
}
