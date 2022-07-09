using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour 
{
    public HeroCtrl heroCtrl;

    [Header("--- JoyStick ---")]
    public GameObject joySBackObj = null;
    public Image joyStickImg = null;
    float radius = 0.0f;
    Vector3 orignPos = Vector3.zero;
    Vector3 axis = Vector3.zero;
    Vector3 jsCacVec = Vector3.zero;
    float jsCacDist = 0.0f;

    private void Start()
    {
        Vector3[] v = new Vector3[4];
        joySBackObj.GetComponent<RectTransform>().GetWorldCorners(v);
        //[0]:�����ϴ� [1]:������� [2]:������� [3]:�����ϴ�
        //v[0] �����ϴ��� 0, 0 ��ǥ�� ��ũ�� ��ǥ(Screen.width, Screen.height)�� ��������   
        radius = v[2].y - v[0].y;
        radius = radius / 3.0f;
        //�߾� ��ġ
        orignPos = joyStickImg.transform.position;
        //��ũ��Ʈ�θ� ����ϰ��� �� �� //�̺�Ʈ �ý��� ���
        EventTrigger trigger = joyStickImg.GetComponent<EventTrigger>(); 
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => {
            OnDragJoyStick((PointerEventData)data);
        });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener((data) => {
            OnEndDragJoyStick((PointerEventData)data);
        });
        trigger.triggers.Add(entry);
    }

    void OnDragJoyStick(PointerEventData _data) //Delegate
    {
        jsCacVec = Input.mousePosition - orignPos;
        jsCacVec.z = 0.0f;
        jsCacDist = jsCacVec.magnitude; // �󸶳� ������
        axis = jsCacVec.normalized; //����Ȯ��

        //���̽�ƽ ��׶��带 ����� ���ϰ� ���� �κ�
        if (radius < jsCacDist)
            joyStickImg.transform.position = orignPos + axis * radius;    
        else
            joyStickImg.transform.position = orignPos + axis * jsCacDist;

        //ĳ���� �̵� ó��
        if (heroCtrl != null)
            heroCtrl.SetJoyStickMv(axis);
    }

    void OnEndDragJoyStick(PointerEventData _data) //Delegate
    {
        axis = Vector3.zero;
        joyStickImg.transform.position = orignPos;

        jsCacDist = 0.0f;

        //ĳ���� ���� ó��
        if (heroCtrl != null)
            heroCtrl.SetJoyStickMv(axis);
    }
}