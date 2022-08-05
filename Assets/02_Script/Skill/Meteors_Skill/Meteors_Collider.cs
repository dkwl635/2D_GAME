using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterHelper;

public class Meteors_Collider : SkillDamageCollider
{
   public AudioSource audioSource;

    Vector2 dir = new Vector2(1.0f, -1.0f).normalized;

    bool move = false;
    private void Update()
    {
        if (move)
            transform.position += (Vector3)dir * Time.deltaTime * 10.0f;
    }

    private void OnEnable()
    {
        collider2D.enabled = false;
        move = true;
    }

    public void Meteors_Evenet()
    {
        audioSource.Play();
        collider2D.enabled = true;
        move = false;
    }
   public void MeteorsEnd_Evenet()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {       
            OnTriggerMonster?.Invoke(collision.GetComponent<ITakeDamage>());
        }
    }
}
