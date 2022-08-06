using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObj : MonoBehaviour
{//코인 오브젝트 클래스
    [SerializeField]private int coin = 1; // 코인 값
    
    public void SetCoin(Vector2 pos) //코인 위치와 활성화 시키기
    {
        gameObject.transform.position = pos;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))//플레이어 체크
        {
            gameObject.SetActive(false);
            GameMgr.Inst.GetCoin(coin); //코인 획득
            GameMgr.Inst.coin_P.ReturnObj(this); 
            //오브젝트풀링을 위해 오브젝트풀에 오브젝트 리턴
        }
    }
}
