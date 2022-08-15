using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;

public class GuidedSword_Collider : SkillDamageCollider
{
    public delegate void GuidedSword_Event(GuidedSword_Collider guidedSword_Collider);
    public GuidedSword_Event DeQuObj; //다시 사용할수 있도록 돌려놓기 위한

    public SpriteRenderer img; //추적칼 이미지
    bool move = false;            //움직이는지 

    public GameObject target;  //타겟
    public float speed = 10.0f; //날아가는 속도
    Transform originPos;          //돌아와야하는 위치
    Vector2 dir = Vector2.zero; //방향   
    private void Update()
    {
        if (move)
        {   //타겟이 활성화 된다면
            if (target.activeSelf)//방향을 정한다
                dir = (target.transform.position - transform.position).normalized;
            else//비활성화 상태 라면 추적칼 사라지게
                gameObject.SetActive(false);

            //이동 및 방향
            transform.position += (Vector3)dir * Time.deltaTime * speed;     
            transform.up = dir; //칼 머리가 이동방향을 향해 돌아간다.

        }

    }
    private void OnEnable()
    {
        //시작 위치 잡기
        transform.position = originPos.position; 
        transform.rotation = Quaternion.identity;      
        //발사전까지 대기
        collider2D.enabled = false;
        move = false;
    }
    private void OnDisable()
    { //움직이고 있는도중 사라지면
        if (move)
        {
            move = false;
            DeQuObj?.Invoke(this); //다시 스킬본체에 돌려주기
        }
       
    }
    public void SetTarget(GameObject target)
    {//타겟 설정시 움직이기 시작
        this.target = target;
        move = true;   
        collider2D.enabled = true;
    }
    
    public void InitSword(Transform origin)
    {//처음 칼 셋팅 
        originPos = origin; //돌아오는 위치
        transform.position = origin.position;
    }

    public void SetSword(Sprite sprite, float speed)
    {//레벨별 이미지와 날아가는 속도 설정을 위한
        img.sprite = sprite;
        this.speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")) //몬스터와 충돌시
        {//데미지 주기
            OnTriggerMonster?.Invoke(collision.GetComponent<ITakeDamage>());          
            target = null;
            gameObject.SetActive(false);
        }

    }

 

}
