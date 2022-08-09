using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardHelp; //ī�� ���� �ʿ� �������̽� ����� ����

public class Card : MonoBehaviour
{
    public Image image; //ī�� �̹���
    public TextMeshProUGUI info;    //ī�� �����

    public void SetCard(CardData card) //ī��⺻ ����
    {
        image.sprite = card.img;
        info.text = card.info;
    }
   
}