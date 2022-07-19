using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Object Asset/MonsterData")]
public class MonsterData : ScriptableObject
{
    public RuntimeAnimatorController monsterAnimator;

    [Header("Colider")]
    public Vector2 offset;
    public float coliderSize;

    [Header("Status")]
    public Vector2 attackBoxSize;
    public int hp;
    public int attDis;
    public int AttPw;
   

}
