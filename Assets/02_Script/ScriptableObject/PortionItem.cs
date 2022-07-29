using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PortionItem", menuName = "Scriptable Object Asset/PortionItem")]
public class PortionItem : ScriptableObject
{
    public Sprite img;  //물건 이미지
    public  AbilityType AbilityType;    //어떤물약인지
    public string itemName; //아이템 이름
    public int value;   //수치
    public int price; //가격


}
