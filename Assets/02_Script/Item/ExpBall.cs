using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpBall : MonoBehaviour
{

    Vector2 originPos; //원래 위치 //스폰된 위치
    Vector2 randPos;   //중심위치에서 랜덤 방향 과 거리를 더한 값
    HeroCtrl hero;      //목적지
   
    bool start = false; //이동을 시작할껀지
    float timer = 0.0f; //이동 계산을 위한

    int exp = 0;    //적용 경험치



    private void Update()
    {
        if (start)
        {
            timer += Time.deltaTime;
            //곡선 이동 시작
            transform.position = Pos(originPos, randPos, hero.transform.position, timer);
                   
            if(timer >= 1.0f)   //이동 완료
            {
                hero.GetExp(exp);//경험치 획득
                GameMgr.Inst.SoundEffectPlay("GetExp"); //효과음
                gameObject.SetActive(false);    //오브젝트 비활성화
                GameMgr.Inst.expBall_P.ReturnObj(this);
                //오브젝트풀링을 위해 오브젝트풀에 오브젝트 리턴
            }
        }           
    }

    public void SetExpBall(Vector2 pos, HeroCtrl hero, int exp)
    {//경험치볼 활성화,위치,목표,
        transform.position = pos; //스폰위치    
        originPos = pos;              
      
        this.exp = exp;     //경험치 수치
        this.hero = hero;   //목표
        //초기화
        timer = 0.0f;          
        start = false;
        gameObject.SetActive(true);
    }

    Vector2 Pos(Vector2 start, Vector2 mid, Vector2 end, float time)    //베지어 곡선을 만들기위한 계산
    {
        Vector2 a = Vector2.Lerp(start,mid,time);
        Vector2 b = Vector2.Lerp(mid, end, time);    
        return Vector2.Lerp(a, b, time);    
    }

    private void OnTriggerEnter2D(Collider2D collision) //사정거리에 히어로가 걸리면
    {
        if (start) return;

        if (collision.CompareTag("Player"))
        {
            //좌표 만들기  .. 곡선효과를 주기위해 랜덤값 만들기
            randPos = (Vector2)transform.position + Random.insideUnitCircle * 5;        
            start = true; //움직이기 시작
        }
    }
}
