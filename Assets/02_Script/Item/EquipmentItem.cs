using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentItem", menuName = "Scriptable Object Asset/EquipmentItem")]

public class EquipmentItem : ScriptableObject
{
    public Sprite[] img;  //��� �̹���
    public EquipmentType Type;    //���������
    public string itemName; //������ �̸�
    
    public int value;   //��ġ
    public int price; //����
}
