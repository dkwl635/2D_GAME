using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour ,IPointerClickHandler
{
  public EquipmentType type; //표현하는 장비타입
    public void OnPointerClick(PointerEventData eventData)
    {//슬롯클릭시 장비설명창 오픈
        GameMgr.Inst.ShowEqItemInfo(type, transform.position);
    }
}
