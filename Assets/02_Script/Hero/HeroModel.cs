using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon_L, //0
    Weapon_R,//1
    Shield, //2
    Plant,   //3哭率 4坷弗率
    Cloth,  //5 啊款单 6哭率 7坷弗率
    Armor,  //8啊款单 9哭率 10坷弗率
  
}

public class HeroModel : MonoBehaviour
{
    public SpriteRenderer[] Parts;

    public void SetEqItem(EquipmentItem item)
    {
        if (item.Type == EquipmentType.Weapon_L)
        {
            Parts[0].sprite = item.img[0];
        }
        else if (item.Type == EquipmentType.Weapon_R)
        {
            Parts[1].sprite = item.img[0];
        }
        else if (item.Type == EquipmentType.Shield)
        {
            Parts[2].sprite = item.img[0];
        }
        else if (item.Type == EquipmentType.Plant)
        {
            Parts[3].sprite = item.img[0];
            Parts[4].sprite = item.img[1];
        }
        else if (item.Type == EquipmentType.Armor)
        {
            Parts[8].sprite = item.img[0];
            Parts[9].sprite = item.img[1];
            Parts[10].sprite = item.img[2];

        }
        
    }

}
