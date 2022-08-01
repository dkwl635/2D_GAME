using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform p1;
    public Transform p2;
    public Transform p3;

    bool a = false;
    float b = 0.0f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            a = true;
        if(a)
        {
            b += Time.deltaTime;
            transform.position = Jump(b);
        }

    }


    public Vector2 Jump(float timer)
    {
        Vector2 a = Vector3.Lerp(p1.position, p2.position, timer);
        Vector2 b = Vector3.Lerp(p2.position, p3.position, timer);

        return Vector2.Lerp(a, b, timer);
    }

}
