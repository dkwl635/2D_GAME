using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCtrl : MonoBehaviour
{
    Transform tr;
    Transform heroModel;
    Vector3 mvDir = Vector3.zero;
    Animator animator;
    [Header("Move")]
    [SerializeField] private float speed = 2;
    private Vector3 originScale;


    private void Awake()
    {
        tr = GetComponent<Transform>();
        heroModel = tr.GetChild(0);
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        originScale = heroModel.localScale;
    }

    private void FixedUpdate()
    {
        tr.position += mvDir * Time.fixedDeltaTime * speed;
    }

    public void SetJoyStickMv(Vector3 dir)
    {
        mvDir = dir;
        
        if (mvDir.Equals(Vector3.zero))
            animator.SetBool("move", false);
        else
            animator.SetBool("move", true);

        if (mvDir.x < 0)
            heroModel.localScale = originScale;
        else if(mvDir.x > 0)
        {
            Vector3 temp = originScale;
            temp.x *= -1;
            heroModel.localScale = temp;
        }
    }

}
