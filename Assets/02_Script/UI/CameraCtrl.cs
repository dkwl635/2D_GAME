using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraCtrl : MonoBehaviour
{
    public Transform targetTr; //쫒아다닐 목표
    private Vector3 cameraPos;
    private Vector3 camWMax;

    private float camSizeX = 0.0f; //카메라뷰의 크기
    private float camSizeY = 0.0f;
    //계산용
    private float xVelocity = 0.0f; 
    private float yVelocity = 0.0f;
    private float smoothTime = 0.2f;

    public Tilemap tilemap; //타일맵(크기를 가져올려고)

    Vector3 mapSize;

    private void Awake()
    {
        cameraPos = transform.position;
    }

    private void Start()
    {
        mapSize.x = tilemap.size.x / 2; //타일맵 사이즈  
        mapSize.y = tilemap.size.y / 2; //시작이 중심(0,0)이기떄문에 나누기2를 했다.

        camWMax = Camera.main.ViewportToWorldPoint(Vector3.one);
    
        camSizeX =  camWMax.x - transform.position.x + 1;
        camSizeY = camWMax.y - transform.position.y + 1;
    }

    private void LateUpdate()
    {
        cameraPos = transform.position; //현재 위치
        //캐릭터의 움직임에 맞춰 이동하기
        cameraPos.x = Mathf.SmoothDamp(transform.position.x, targetTr.position.x, ref xVelocity, smoothTime);
        cameraPos.y = Mathf.SmoothDamp(transform.position.y, targetTr.position.y, ref yVelocity, smoothTime);

        //카메라 지형 밖으로 안나가게 //최대 최솟값 좌표 구하기
        if (cameraPos.x + camSizeX > mapSize.x)
            cameraPos.x = mapSize.x - camSizeX;

        if (cameraPos.x - camSizeX < -mapSize.x)
            cameraPos.x = -mapSize.x + camSizeX;

        if (cameraPos.y + camSizeY > mapSize.y)
            cameraPos.y = mapSize.y - camSizeY;

        if (cameraPos.y - camSizeY < -mapSize.y)
            cameraPos.y = -mapSize.y + camSizeY;
        //위치 값
        transform.position = cameraPos;
    }
}
