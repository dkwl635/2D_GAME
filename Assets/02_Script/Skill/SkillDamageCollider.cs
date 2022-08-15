using MonsterHelper;
using UnityEngine;


public class SkillDamageCollider : MonoBehaviour
{  //스킬의 데미지 판정에 도움을주는 콜라이더 클래스 
    
    public delegate void Event(ITakeDamage monster);
    public Event OnTriggerMonster;

    protected Collider2D collider2D;

    public virtual void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

}
