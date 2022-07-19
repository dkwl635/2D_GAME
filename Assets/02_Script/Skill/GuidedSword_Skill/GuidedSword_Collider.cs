using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedSword_Collider : SkillDamageCollider
{
    public delegate void GuidedSword_Event(GuidedSword_Collider guidedSword_Collider);
    public GuidedSword_Event DeQuObj;


    SpriteRenderer img;
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
            if (!target.Equals(null) && target.hp > 0)
            {
                dir = (target.transform.position - transform.position).normalized;
                target = null;
            }

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
    
    public void SetSword(Transform origin)
    {
        originPos = origin;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            OnTriggerMonster?.Invoke(collision.GetComponent<Monster>());
            gameObject.SetActive(false);
        }

    }

    Vector2 Pos(Vector2 start, Vector2 mid, Vector2 end, float time)    
    {
        Vector2 a = Vector2.Lerp(start, mid, time);
        Vector2 b = Vector2.Lerp(mid, end, time);
        return Vector2.Lerp(a, b, time);
    }

}
