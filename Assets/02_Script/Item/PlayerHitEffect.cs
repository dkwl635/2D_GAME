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
    HitType hitType = HitType.none;//������ Ÿ��
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

        //������ Ÿ�Ժ� ���� �� ������ ����Ʈ ����
        if (hitType.Equals(HitType.nomarl))
        {
            spriteRenderer.color = Color.red;
            animator.SetTrigger("Normal");
        }
    }

    public void SetEffect(Vector3 pos, HitType hitType)
    {//����Ʈ Ȱ��ȭ
        transform.position = pos;
        this.hitType = hitType;
        transform.gameObject.SetActive(true);
   
    }

    public void EndEffect_Event()
    {//����Ʈ ����� �ִϸ��̼ǿ� ��ϵǴ� �̺�Ʈ �Լ�
        gameObject.SetActive(false);
        GameMgr.Inst.playerHitEffect_P.ReturnObj(this);
        //������ƮǮ���� ���� ������Ʈ ����
    }

}
