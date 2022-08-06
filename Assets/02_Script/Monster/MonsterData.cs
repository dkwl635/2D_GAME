using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHelp;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Object Asset/MonsterData")]
public class MonsterData : ScriptableObject , SetCard
{//몬스터 데이터 ScriptableObject
    public Sprite monsterImg; //몬스터 카드 이미지
    public RuntimeAnimatorController monsterAnimator; //애니메이터
    [Header("Colider")]
    public Vector2 offset;
    public float coliderSize;
    [Header("Status")]
    public Vector2 attackBoxSize;
    public int hp;
    public int AttPw;
    public float Speed;

    public CardData GetCard() //카드 설명
    {
        CardData card;
        card.img = monsterImg;
        card.info = "Hp : " + hp + "\nAttack : " + AttPw;
        return card;
    }
}
