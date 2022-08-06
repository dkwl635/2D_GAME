using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObj : MonoBehaviour
{//���� ������Ʈ Ŭ����
    [SerializeField]private int coin = 1; // ���� ��
    
    public void SetCoin(Vector2 pos) //���� ��ġ�� Ȱ��ȭ ��Ű��
    {
        gameObject.transform.position = pos;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))//�÷��̾� üũ
        {
            gameObject.SetActive(false);
            GameMgr.Inst.GetCoin(coin); //���� ȹ��
            GameMgr.Inst.coin_P.ReturnObj(this); 
            //������ƮǮ���� ���� ������ƮǮ�� ������Ʈ ����
        }
    }
}
