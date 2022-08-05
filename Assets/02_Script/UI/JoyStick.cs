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
        //[0]:�����ϴ� [1]:������� [2]:������� [3]:�����ϴ�
        //v[0] �����ϴ��� 0, 0 ��ǥ�� ��ũ�� ��ǥ(Screen.width, Screen.height)�� ��������   
        radius = v[2].y - v[0].y;
        radius = radius / 3.0f;
        //�߾� ��ġ
        orignPos = joyStickImg.transform.position;

        #region 1�� ���̽�ƽ�Լ�
        //��ũ��Ʈ�θ� ����ϰ��� �� �� //�̺�Ʈ �ý��� ���
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

    #region 1�� ���̽�ƽ �Լ�
    //void OnDragJoyStick(PointerEventData _data) //Delegate
    //{
    //    jsCacVec = Input.mousePosition - orignPos;
    //    jsCacVec.z = 0.0f;
    //    jsCacDist = jsCacVec.magnitude; // �󸶳� ������
    //    axis = jsCacVec.normalized; //����Ȯ��

    //    //���̽�ƽ ��׶��带 ����� ���ϰ� ���� �κ�
    //    if (radius < jsCacDist)
    //        joyStickImg.transform.position = orignPos + axis * radius;    
    //    else
    //        joyStickImg.transform.position = orignPos + axis * jsCacDist;

    //    //�󸶳� ��ƽ�� ���������� ���� �޸��� ����
    //    bool sprint = false;
    //    if (radius * 0.7 < jsCacDist)
    //        sprint = true;

    //    //ĳ���� �̵� ó��
    //    if (heroCtrl != null)
    //        heroCtrl.SetJoyStickMv(axis, sprint);
    //}

    //void OnEndDragJoyStick(PointerEventData _data) //Delegate
    //{
    //    axis = Vector3.zero;
    //    joyStickImg.transform.position = orignPos;

    //    jsCacDist = 0.0f;

    //    //ĳ���� ���� ó��
    //    if (heroCtrl != null)
    //        heroCtrl.SetJoyStickMv(axis);
    //}
    #endregion 

    public void OnDrag(PointerEventData eventData)
    {
        jsCacVec = eventData.position - orignPos;
        jsCacDist = jsCacVec.magnitude; // �󸶳� ������
        axis = jsCacVec.normalized; //����Ȯ��

        //���̽�ƽ ��׶��带 ����� ���ϰ� ���� �κ�
        if (radius < jsCacDist)
            joyStickImg.transform.position = orignPos + axis * radius;
        else
            joyStickImg.transform.position = orignPos + axis * jsCacDist;

        //�󸶳� ��ƽ�� ���������� ���� �޸��� ����
        bool sprint = false;
        if (radius * 0.7 < jsCacDist)
            sprint = true;

        //ĳ���� �̵� ó��
        if (heroCtrl != null)
            heroCtrl.SetJoyStickMv(axis, sprint);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        axis = Vector3.zero;
        joyStickImg.transform.position = orignPos;

        jsCacDist = 0.0f;

        //ĳ���� ���� ó��
        if (heroCtrl != null)
            heroCtrl.SetJoyStickMv(axis);
    }
}
