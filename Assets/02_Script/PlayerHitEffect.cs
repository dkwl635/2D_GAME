using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType
{
    none,
    nomarl
}

public class PlayerHitEffect : MonoBehaviour
{
    HitType hitType = HitType.none;

    Animator animator;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.color = Color.white;

        if (hitType.Equals(HitType.nomarl))
        {
            spriteRenderer.color = Color.red;
            animator.SetTrigger("Normal");
        }
    }

    public void SetEffect(Vector3 pos, HitType hitType)
    {
        transform.position = pos;
        this.hitType = hitType;
        transform.gameObject.SetActive(true);
   
    }

    public void EndEffect_Event()
    {
        gameObject.SetActive(false);
        GameMgr.Inst.playerHitEffect_P.ReturnObj(this);   
    }

}
