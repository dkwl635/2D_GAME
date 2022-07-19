using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_Collider : SkillDamageCollider
{
     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            OnTriggerMonster?.Invoke(collision.GetComponent<Monster>());
        }
    }
}
