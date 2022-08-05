using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Object Asset/StageData")]

public class StageData : ScriptableObject
{
    public MonsterData[] monsterDatas;  //스테이지에서 나타낼 몬스터 종류
    public int monsterCount;      //스테이지에서 나타낼 몬스터 갯수

    public GameObject bossMonsterPrefab;
}
