using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour ,  IDragHandler, IPointerUpHandler
{
    HeroCtrl heroCtrl;

    [Header("--- JoyStick ---")]
    public GameObject joySBackObj = null;
    public Image joyStickImg = null;
    float radius = 0.0f;
    Vector2 orignPos = Vector3.zero;
    Vector2 axis = Vector3.zero;
    Vector2 jsCacVec = Vector3.zero;
    float jsCacDist = 0.0f;

    private void Start()
    {
        heroCtrl = GameMgr.Inst.hero;

        Vector3[] v = new Vector3[4];
        joySBackObj.GetComponent<RectTransform>().GetWorldCorners(v);
        //[0]:좌측하단 [1]:좌측상단 [2]:우측상단 [3]:우측하단
        //v[0] 촤측하단이 0, 0 좌표인 스크린 좌표(Screen.width, Screen.height)를 기준으로   
        radius = v[2].y - v[0].y;
        radius = radius / 3.0f;
        //중앙 위치
        orignPos = joyStickImg.transform.position;

        #region 1차 조이스틱함수
        //스크립트로만 대기하고자 할 때 //이벤트 시스템 등록
        //EventTrigger trigger = joyStickImg.GetComponent<EventTrigger>(); 
        //EventTrigger.Entry entry = new EventTrigger.Entry();
        //entry.eventID = EventTriggerType.Drag;
        //entry.callback.AddListener((data) => {
        //    OnDragJoyStick((PointerEventData)data);
        //});
        //trigger.triggers.Add(entry);

        //entry = new EventTrigger.Entry();
        //entry.eventID = EventTriggerType.EndDrag;
        //entry.callback.AddListener((data) => {
        //    OnEndDragJoyStick((PointerEventData)data);
        //});
        //trigger.triggers.Add(entry);
        #endregion

    }

    #region 1차 조이스틱 함수
    //void OnDragJoyStick(PointerEventData _data) //Delegate
    //{
    //    jsCacVec = Input.mousePosition - orignPos;
    //    jsCacVec.z = 0.0f;
    //    jsCacDist = jsCacVec.magnitude; // 얼마나 갔는지
    //    axis = jsCacVec.normalized; //방향확인

    //    //조이스틱 백그라운드를 벗어나지 못하게 막는 부분
    //    if (radius < jsCacDist)
    //        joyStickImg.transform.position = orignPos + axis * radius;    
    //    else
    //        joyStickImg.transform.position = orignPos + axis * jsCacDist;

    //    //얼마나 스틱을 움직였나에 따라 달리기 가능
    //    bool sprint = false;
    //    if (radius * 0.7 < jsCacDist)
    //        sprint = true;

    //    //캐릭터 이동 처리
    //    if (heroCtrl != null)
    //        heroCtrl.SetJoyStickMv(axis, sprint);
    //}

    //void OnEndDragJoyStick(PointerEventData _data) //Delegate
    //{
    //    axis = Vector3.zero;
    //    joyStickImg.transform.position = orignPos;

    //    jsCacDist = 0.0f;

    //    //캐릭터 정지 처리
    //    if (heroCtrl != null)
    //        heroCtrl.SetJoyStickMv(axis);
    //}
    #endregion 

    public void OnDrag(PointerEventData eventData)
    {
        jsCacVec = eventData.position - orignPos;
        jsCacDist = jsCacVec.magnitude; // 얼마나 갔는지
        axis = jsCacVec.normalized; //방향확인

        //조이스틱 백그라운드를 벗어나지 못하게 막는 부분
        if (radius < jsCacDist)
            joyStickImg.transform.position = orignPos + axis * radius;
        else
            joyStickImg.transform.position = orignPos + axis * jsCacDist;

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
        axis = Vector3.zero;
        joyStickImg.transform.position = orignPos;

        jsCacDist = 0.0f;

        //캐릭터 정지 처리
        if (heroCtrl != null)
            heroCtrl.SetJoyStickMv(axis);
    }
}
