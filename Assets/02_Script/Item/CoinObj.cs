using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObj : MonoBehaviour
{
    public int coin = 1;

    public void SetCoin(Vector2 pos)
    {
        gameObject.transform.position = pos;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            GameMgr.Inst.GetCoin(coin);
            GameMgr.Inst.coin_P.ReturnObj(this);
        }
    }
}
