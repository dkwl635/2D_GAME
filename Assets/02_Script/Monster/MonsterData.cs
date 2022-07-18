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
    public int hp;
    public int AttPw;
   

}
