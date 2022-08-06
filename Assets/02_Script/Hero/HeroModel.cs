using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType //장비 타입
{//숫자 = 파트별 이미지 위치
    Weapon_L, //0
    Weapon_R,//1
    Shield, //2
    Plant,   //3왼쪽 4오른쪽
    Cloth,  //5 가운데 6왼쪽 7오른쪽
    Armor,  //8가운데 9왼쪽 10오른쪽  
}

public class HeroModel : MonoBehaviour
{
    public SpriteRenderer[] Parts; //장비 이미지 배열

    public void SetEqItem(EquipmentItem item)
    {
        //타입별 이미지위치에 교체
        if (item.Type == EquipmentType.Weapon_L)        
            Parts[0].sprite = item.img[0];    
        else if (item.Type == EquipmentType.Weapon_R)       
            Parts[1].sprite = item.img[0];       
        else if (item.Type == EquipmentType.Shield)     
            Parts[2].sprite = item.img[0];      
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
