using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardHelp //ī��� �����ټ��ְ� ���ִ�
{
    public struct CardData //ī�� ������
    {
        public Sprite img;
        public string info;
    }

    public interface ICardLvUp //�������� ������ ī���
    {
        public bool LevelPossible();
        public void LevelUp();
        public CardData GetCard();
    }

    public interface SetCard //�ܼ��� ī�� ������ �����ټ� �ְ��ϴ�
    {    
        public CardData GetCard();
    }
}

namespace MonsterHelper  //���Ϳ��� �ٿ��ִ� �������̽��Լ�
{
    public interface ITakeDamage//���� �������� �޴� �������̽��Լ�
    {
        public void TakeDamage(int value);
    }
}