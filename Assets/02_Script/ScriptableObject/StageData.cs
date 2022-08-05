using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Object Asset/StageData")]

public class StageData : ScriptableObject
{
    public MonsterData[] monsterDatas;  //������������ ��Ÿ�� ���� ����
    public int monsterCount;      //������������ ��Ÿ�� ���� ����

    public GameObject bossMonsterPrefab;
}
