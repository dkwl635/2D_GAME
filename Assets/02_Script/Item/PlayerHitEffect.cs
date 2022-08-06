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
    HitType hitType = HitType.none;//데미지 타입
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

        //데미지 타입별 색상 및 데미지 이펙트 결정
        if (hitType.Equals(HitType.nomarl))
        {
            spriteRenderer.color = Color.red;
            animator.SetTrigger("Normal");
        }
    }

    public void SetEffect(Vector3 pos, HitType hitType)
    {//이펙트 활성화
        transform.position = pos;
        this.hitType = hitType;
        transform.gameObject.SetActive(true);
   
    }

    public void EndEffect_Event()
    {//이펙트 종료시 애니메이션에 등록되는 이벤트 함수
        gameObject.SetActive(false);
        GameMgr.Inst.playerHitEffect_P.ReturnObj(this);
        //오브젝트풀링을 위한 오브젝트 리턴
    }

}
