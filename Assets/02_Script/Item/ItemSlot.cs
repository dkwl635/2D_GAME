using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour ,IPointerClickHandler
{
  public EquipmentType type;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameMgr.Inst.ShowEqItemInfo(type, transform.position);
    }
}
