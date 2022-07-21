using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricball_Collider : SkillDamageCollider
{
    public float speed = 1.0f;
    Vector3 pos = Vector3.zero;
 
    private void Update()
    {
        pos.x = 5 * Mathf.Sin(Time.time * speed) ;
        transform.localPosition = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            OnTriggerMonster?.Invoke(collision.GetComponent<Monster>());
        }
    }

}
