using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentItem", menuName = "Scriptable Object Asset/EquipmentItem")]

public class EquipmentItem : ScriptableObject
{
    public Sprite[] img;  //장비 이미지
    public EquipmentType Type;    //어떤물약인지
    public string itemName; //아이템 이름
    
    public int value;   //수치
    public int price; //가격
}
