using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillDamageCollider : MonoBehaviour
{   
    public delegate void Event(Monster monster);
    public Event OnTriggerMonster;

   protected Collider2D collider2D;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

}
