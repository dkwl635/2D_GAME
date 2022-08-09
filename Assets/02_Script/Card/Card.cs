using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardHelp; //카드 관련 필요 인터페이스 사용을 위해

public class Card : MonoBehaviour
{
    public Image image; //카드 이미지
    public TextMeshProUGUI info;    //카드 설명란

    public void SetCard(CardData card) //카드기본 셋팅
    {
        image.sprite = card.img;
        info.text = card.info;
    }
   
}