using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraCtrl : MonoBehaviour
{
    public Transform targetTr; //�i�ƴٴ� ��ǥ
    private Vector3 cameraPos;
    private Vector3 camWMax;

    private float camSizeX = 0.0f; //ī�޶���� ũ��
    private float camSizeY = 0.0f;
    //����
    private float xVelocity = 0.0f; 
    private float yVelocity = 0.0f;
    private float smoothTime = 0.2f;

    public Tilemap tilemap; //Ÿ�ϸ�(ũ�⸦ �����÷���)

    Vector3 mapSize;

    private void Awake()
    {
        cameraPos = transform.position;
    }

    private void Start()
    {
        mapSize.x = tilemap.size.x / 2; //Ÿ�ϸ� ������  
        mapSize.y = tilemap.size.y / 2; //������ �߽�(0,0)�̱⋚���� ������2�� �ߴ�.

        camWMax = Camera.main.ViewportToWorldPoint(Vector3.one);
    
        camSizeX =  camWMax.x - transform.position.x + 1;
        camSizeY = camWMax.y - transform.position.y + 1;
    }

    private void LateUpdate()
    {
        cameraPos = transform.position; //���� ��ġ
        //ĳ������ �����ӿ� ���� �̵��ϱ�
        cameraPos.x = Mathf.SmoothDamp(transform.position.x, targetTr.position.x, ref xVelocity, smoothTime);
        cameraPos.y = Mathf.SmoothDamp(transform.position.y, targetTr.position.y, ref yVelocity, smoothTime);

        //ī�޶� ���� ������ �ȳ����� //�ִ� �ּڰ� ��ǥ ���ϱ�
        if (cameraPos.x + camSizeX > mapSize.x)
            cameraPos.x = mapSize.x - camSizeX;

        if (cameraPos.x - camSizeX < -mapSize.x)
            cameraPos.x = -mapSize.x + camSizeX;

        if (cameraPos.y + camSizeY > mapSize.y)
            cameraPos.y = mapSize.y - camSizeY;

        if (cameraPos.y - camSizeY < -mapSize.y)
            cameraPos.y = -mapSize.y + camSizeY;
        //��ġ ��
        transform.position = cameraPos;
    }
}
