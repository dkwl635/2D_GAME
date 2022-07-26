using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PortionItem", menuName = "Scriptable Object Asset/PortionItem")]
public class PortionItem : ScriptableObject
{
    public Sprite img;  //���� �̹���
    public  AbilityType AbilityType;    //���������
    public string itemName; //������ �̸�
    public int value;   //��ġ
    public int price; //����


}
