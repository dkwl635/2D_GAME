using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Collider : MonoBehaviour
{
    Rigidbody2D rigidbody; //��ġ�� ���� ���� �߰�
    public float speed = 0; //�ӵ�

    Vector2 dir; //��ġ�� ����

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

        //���⿡ ���� ������Ʈ �������
        if (dir.x < 0)
            transform.right = -dir;
        else
            transform.right = dir;

        rigidbody.velocity = dir * speed;
    }


}
