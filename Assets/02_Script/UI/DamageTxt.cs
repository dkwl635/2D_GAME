using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageTxt : MonoBehaviour
{
   public  TextMeshPro textMeshPro;//데미지 수치를 나타낼 
    //DamageTxt는 애니메이션을 만들어 효과를 주었음

    private void OnEnable()
    {   //폰트 사이즈 초기화
        textMeshPro.fontSize = 1;
    }

    private void Update()
    {
        //애니메이션을 이용하여 폰트사이즈를 가지고 효과를 만듬
        //폰트사이즈 0이면 애니메이션이 끝난것과 같음
        if (textMeshPro.fontSize.Equals(0))
            OffDamageTxt();
    }

    public void SetDamageTxt(int value, Vector3 pos)
    {//외부에서 텍스트를 설정하는 함수
        transform.position = pos; 
        textMeshPro.text = value.ToString();
        //오브젝트 활성화시 애니메이션 자동실행이 됨
        gameObject.SetActive(true);
    }

    void OffDamageTxt()
    {//오브젝트 종료시
        gameObject.SetActive(false);
        //다시 재활용을 위해 오브젝트 리턴
        GameMgr.Inst.DamageTxtEffect_P.ReturnObj(this);
    }
}
