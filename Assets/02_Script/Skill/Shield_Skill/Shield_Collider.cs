using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Collider : MonoBehaviour
{
    Rigidbody2D rigidbody; //밀치기 위한 물리 추가
    public float speed = 0; //속도

    Vector2 dir; //밀치는 방향

    // Start is called before the first frame update
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void SpawnShield(Vector2 spawnPos, Vector2 dir)
    {
        this.dir = dir;
        transform.position = spawnPos;
        gameObject.SetActive(true);

        //방향에 따른 오브젝트 방향결정
        if (dir.x < 0)
            transform.right = -dir;
        else
            transform.right = dir;

        rigidbody.velocity = dir * speed;
    }


}
