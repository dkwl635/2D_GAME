using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour ,  IDragHandler, IPointerUpHandler
{
    //IDragHandler 마우스 드래그
    //IPointerUpHandler 마우스 클릭 후 땔떄
    HeroCtrl heroCtrl; 
    [Header("--- JoyStick ---")]
    public GameObject joySBackObj = null;
    float radius = 0.0f;
    Vector2 orignPos = Vector3.zero;
    Vector2 axis = Vector3.zero;
    Vector2 jsCacVec = Vector3.zero;
    float jsCacDist = 0.0f;

    private void Start()
    {
        heroCtrl = GameMgr.Inst.hero; //캐릭터연결
        Vector3[] v = new Vector3[4];
        joySBackObj.GetComponent<RectTransform>().GetWorldCorners(v);
        //[0]:좌측하단 [1]:좌측상단 [2]:우측상단 [3]:우측하단
        //v[0] 촤측하단이 0, 0 좌표인 스크린 좌표(Screen.width, Screen.height)를 기준으로   
        radius = v[2].y - v[0].y;
        radius = radius / 3.0f;
        //중앙 위치
        orignPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {  //IDragHandler 마우스 드래그

        jsCacVec = eventData.position - orignPos;
        jsCacDist = jsCacVec.magnitude; // 얼마나 갔는지
        axis = jsCacVec.normalized; //방향확인

        //조이스틱 백그라운드를 벗어나지 못하게 막는 부분
        if (radius < jsCacDist)
            transform.position = orignPos + axis * radius;
        else
            transform.position = orignPos + axis * jsCacDist;

        //얼마나 스틱을 움직였나에 따라 달리기 가능
        bool sprint = false;
        if (radius * 0.7 < jsCacDist)
            sprint = true;

        //캐릭터 이동 처리
        if (heroCtrl != null)
            heroCtrl.SetJoyStickMv(axis, sprint);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        axis = Vector3.zero; //초기화
        jsCacDist = 0.0f;
        transform.position = orignPos; //원래 위치로

        //캐릭터 정지 처리
        if (heroCtrl != null)
            heroCtrl.SetJoyStickMv(axis);
    }
}
