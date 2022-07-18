using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpBall : MonoBehaviour
{

    Vector2 originPos; //���� ��ġ //������ ��ġ
    Vector2 randPos;   //�߽���ġ���� ���� ���� �� �Ÿ��� ���� ��
    HeroCtrl hero;      //������
   
    bool start = false; //�̵��� �����Ҳ���
    float timer = 0.0f; //�̵� ����� ����

    int exp = 0;    //���� ����ġ

    void Start()
    {
        originPos = transform.position; //���� ��ġ ����
    }

    private void Update()
    {
        if (start)
        {
            timer += Time.deltaTime;
            //� �̵� ����
            transform.position = Pos(originPos, randPos, hero.transform.position, timer);
                   
            if(timer >= 1.0f)   //�̵� �Ϸ�
            {
                hero.GetExp(exp);//����ġ 
                Destroy(this.gameObject);
            }
        }           
    }

    public void SetExpBall(HeroCtrl hero, int exp)
    {
        this.exp = exp;
        this.hero = hero;
    }

    Vector2 Pos(Vector2 start, Vector2 mid, Vector2 end, float time)    //������ ��� ��������� ���
    {
        Vector2 a = Vector2.Lerp(start,mid,time);
        Vector2 b = Vector2.Lerp(mid, end, time);    
        return Vector2.Lerp(a, b, time);    
    }

    private void OnTriggerEnter2D(Collider2D collision) //�����Ÿ��� ����ΰ� �ɸ���
    {
        if (start) return;

        if (collision.CompareTag("Player"))
        {
            randPos = (Vector2)transform.position + Random.insideUnitCircle * 5; //��ǥ �����
            start = true;
        }

       
    }

  

}
