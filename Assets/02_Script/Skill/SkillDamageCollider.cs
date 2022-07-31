using MonsterHelper;
using UnityEngine;


public class SkillDamageCollider : MonoBehaviour
{   
    public delegate void Event(ITakeDamage monster);
    public Event OnTriggerMonster;

   protected Collider2D collider2D;

    public virtual void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

}
