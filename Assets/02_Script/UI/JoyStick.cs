using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour ,  IDragHandler, IPointerUpHandler
{
    //IDragHandler ���콺 �巡��
    //IPointerUpHandler ���콺 Ŭ�� �� ����
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
        heroCtrl = GameMgr.Inst.hero; //ĳ���Ϳ���
        Vector3[] v = new Vector3[4];
        joySBackObj.GetComponent<RectTransform>().GetWorldCorners(v);
        //[0]:�����ϴ� [1]:������� [2]:������� [3]:�����ϴ�
        //v[0] �����ϴ��� 0, 0 ��ǥ�� ��ũ�� ��ǥ(Screen.width, Screen.height)�� ��������   
        radius = v[2].y - v[0].y;
        radius = radius / 3.0f;
        //�߾� ��ġ
        orignPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {  //IDragHandler ���콺 �巡��

        jsCacVec = eventData.position - orignPos;
        jsCacDist = jsCacVec.magnitude; // �󸶳� ������
        axis = jsCacVec.normalized; //����Ȯ��

        //���̽�ƽ ��׶��带 ����� ���ϰ� ���� �κ�
        if (radius < jsCacDist)
            transform.position = orignPos + axis * radius;
        else
            transform.position = orignPos + axis * jsCacDist;

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
        axis = Vector3.zero; //�ʱ�ȭ
        jsCacDist = 0.0f;
        transform.position = orignPos; //���� ��ġ��

        //ĳ���� ���� ó��
        if (heroCtrl != null)
            heroCtrl.SetJoyStickMv(axis);
    }
}
