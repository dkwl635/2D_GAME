using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardHelp
{
    public struct CardData
    {
        public Sprite img;
        public string info;
    }

    public interface ICardLvUp
    {
        public bool LevelPossible();
        public void LevelUp();
        public CardData GetCard();
    }

    public interface SetCard
    {    
        public CardData GetCard();
    }
}

