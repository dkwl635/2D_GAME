using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopOpen : MonoBehaviour ,IPointerClickHandler
{//���� ������Ʈ �پ��ִ½�ũ��
    public void OnPointerClick(PointerEventData eventData) //�ݶ��̴��� �����Ͽ� Ŭ����
    {//����UI ����
        ShopMgr.Inst.ShopOpen();
    }

  
}
