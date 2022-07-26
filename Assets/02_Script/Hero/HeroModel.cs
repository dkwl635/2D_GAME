using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon_L, //0
    Weapon_R,//1
    Shield, //2
    Foot,   //3 4
    Cloth,  //5
    Armor,  //6
    Arm,    //7 8
}

public class HeroModel : MonoBehaviour
{
    public SpriteRenderer[] Parts;
}
