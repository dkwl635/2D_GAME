using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedSword_Collider : SkillDamageCollider
{
    public delegate void GuidedSword_Event(GuidedSword_Collider guidedSword_Collider);
    public GuidedSword_Event DeQuObj;


    public SpriteRenderer img;
    bool move = false;

    public Monster target;
    public float speed = 10.0f;
    Transform originPos;
    Vector2 dir = Vector2.zero;

    public float lifeTime = 3.0f;
   
    private void Update()
    {
        if(move)
        {
            if (target && target.hp > 0)
            {
                dir = (target.transform.position - transform.position).normalized;     
            }
            else target = null;

            transform.position += (Vector3)dir * Time.deltaTime  * speed;

            transform.up = dir;

            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
                gameObject.SetActive(false);
        }


    }

    private void OnEnable()
    {
        transform.SetPositionAndRotation(originPos.position, Quaternion.identity);       
        collider2D.enabled = false;
        move = false;
    }

    private void OnDisable()
    {
        if(move)
        {
            move = false;
            DeQuObj?.Invoke(this);
        }
       
    }

    public void SetTarget(Monster monster)
    {
        target = monster;
        move = true;
        lifeTime = 3.0f;
        collider2D.enabled = true;
    }
    
    public void InitSword(Transform origin)
    {
        originPos = origin;
    }

    public void SetSword(Sprite sprite, float speed)
    {
        img.sprite = sprite;
        this.speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            OnTriggerMonster?.Invoke(collision.GetComponent<Monster>());          
            target = null;
            gameObject.SetActive(false);
        }

    }

 

}
