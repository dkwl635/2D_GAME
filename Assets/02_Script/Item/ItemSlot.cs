using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour ,IPointerClickHandler
{
  public EquipmentType type; //ǥ���ϴ� ���Ÿ��
    public void OnPointerClick(PointerEventData eventData)
    {//����Ŭ���� ��񼳸�â ����
        GameMgr.Inst.ShowEqItemInfo(type, transform.position);
    }
}
