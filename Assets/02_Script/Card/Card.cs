using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardHelp;

public class Card : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI info;

  
    public void SetCard(CardData card)
    {
        image.sprite = card.img;
        info.text = card.info;
    }



}