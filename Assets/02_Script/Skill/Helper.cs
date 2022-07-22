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

    public interface SetCard
    {    
        public CardData GetCard();
    }
}

namespace LavelUpCard
{
    public interface LevelUp
    {
        public bool LevelPossible();
        public void LevelUp();
    }
}