using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopOpen : MonoBehaviour ,IPointerClickHandler
{//상점 오브젝트 붙어있는스크립
    public void OnPointerClick(PointerEventData eventData) //콜라이더에 반응하여 클릭시
    {//상점UI 오픈
        ShopMgr.Inst.ShopOpen();
    }

  
}
