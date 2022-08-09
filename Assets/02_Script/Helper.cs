using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardHelp //카드로 보여줄수있게 해주는
{
    public struct CardData //카드 데이터
    {
        public Sprite img;
        public string info;
    }

    public interface ICardLvUp //레벨업이 가능한 카드들
    {
        public bool LevelPossible();
        public void LevelUp();
        public CardData GetCard();
    }

    public interface SetCard //단순한 카드 정보를 보여줄수 있게하는
    {    
        public CardData GetCard();
    }
}

namespace MonsterHelper  //몬스터에게 붙여주는 인터페이스함수
{
    public interface ITakeDamage//몬스터 데미지를 받는 인터페이스함수
    {
        public void TakeDamage(int value);
    }
}