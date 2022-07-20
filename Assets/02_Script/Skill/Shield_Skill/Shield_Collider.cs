using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Collider : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public float speed = 0;

    Vector2 dir;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(dir.x <0)
         transform.right = -dir;
        else
            transform.right = dir;

        rigidbody.velocity = dir * speed;
       
    }
}
