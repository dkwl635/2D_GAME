using MonsterHelper;
using UnityEngine;

public class Spear_Collider : SkillDamageCollider
{
     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            OnTriggerMonster?.Invoke(collision.GetComponent<ITakeDamage>());
        }
    }
}
