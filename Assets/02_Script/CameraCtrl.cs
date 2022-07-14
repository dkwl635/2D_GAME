using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraCtrl : MonoBehaviour
{
    public Transform targetTr;

    public Vector3 cameraPos;
    public Vector3 camWMin;
    public Vector3 camWMax;

    float camSizeX = 0.0f;
    float camSizeY = 0.0f;

    private float xVelocity = 0.0f;
    private float yVelocity = 0.0f;
    private float smoothTime = 0.2f;

    public Tilemap tilemap;

    Vector3 mapSize;

    private void Awake()
    {
        cameraPos = transform.position;
    }

    private void Start()
    {
        mapSize.x = tilemap.size.x / 2;
        mapSize.y = tilemap.size.y / 2;
        mapSize.z = tilemap.size.z / 2;

        camWMin = Camera.main.ViewportToWorldPoint(Vector3.zero);
        camWMax = Camera.main.ViewportToWorldPoint(Vector3.one);

        
        camSizeX =  camWMax.x - transform.position.x + 4;
        camSizeY = camWMax.y - transform.position.y + 4;
    }

    private void LateUpdate()
    {
        cameraPos = transform.position;

        cameraPos.x = Mathf.SmoothDamp(transform.position.x,

                    targetTr.position.x, ref xVelocity, smoothTime);

        cameraPos.y = Mathf.SmoothDamp(transform.position.y,

                    targetTr.position.y, ref yVelocity, smoothTime);

        //카메라 지형 밖으로 안나가게

        if (cameraPos.x + camSizeX > mapSize.x)
            cameraPos.x = mapSize.x - camSizeX;

        if (cameraPos.x - camSizeX < -mapSize.x)
            cameraPos.x = -mapSize.x + camSizeX;

        if (cameraPos.y + camSizeY > mapSize.y)
            cameraPos.y = mapSize.y - camSizeY;

        if (cameraPos.y - camSizeY < -mapSize.y)
            cameraPos.y = -mapSize.y + camSizeY;


        transform.position = cameraPos;

    
    }

}
