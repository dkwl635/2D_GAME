using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHelp;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Object Asset/MonsterData")]
public class MonsterData : ScriptableObject , SetCard
{
    public Sprite monsterImg;
    
    public RuntimeAnimatorController monsterAnimator;

    [Header("Colider")]
    public Vector2 offset;
    public float coliderSize;

    [Header("Status")]
    public Vector2 attackBoxSize;
    public int hp;
    public int AttPw;
    public float Speed;

    public CardData GetCard()
    {
        CardData card;
        card.img = monsterImg;
        card.info = "Hp : " + hp + "\nAttack : " + AttPw;
        return card;
    }
}
