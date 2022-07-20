using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark_Collider : SkillDamageCollider
{
    Vector2 dir;
    Animator animator;

    float lifeTime = 3.0f;
    float speed = 10.0f;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

    
    }

    private void OnEnable()
    {
        collider2D.enabled = true;
        speed = 10.0f;
        lifeTime = 3.0f;
    }

    private void Update()
    {
        transform.right = dir;
        transform.position += (Vector3)dir * Time.deltaTime * speed;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
            gameObject.SetActive(false);
    }

    public void SetSpark(Vector2 dir)
    {
        this.dir = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            OnTriggerMonster?.Invoke(collision.GetComponent<Monster>());
            collider2D.enabled = false;
            animator.SetTrigger("Hit");
            speed = 0.0f;

            lifeTime = 0.5f;
        }
    }

}
