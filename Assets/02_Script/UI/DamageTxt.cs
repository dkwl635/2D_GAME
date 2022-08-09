using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageTxt : MonoBehaviour
{
   public  TextMeshPro textMeshPro;//������ ��ġ�� ��Ÿ�� 
    //DamageTxt�� �ִϸ��̼��� ����� ȿ���� �־���

    private void OnEnable()
    {   //��Ʈ ������ �ʱ�ȭ
        textMeshPro.fontSize = 1;
    }

    private void Update()
    {
        //�ִϸ��̼��� �̿��Ͽ� ��Ʈ����� ������ ȿ���� ����
        //��Ʈ������ 0�̸� �ִϸ��̼��� �����Ͱ� ����
        if (textMeshPro.fontSize.Equals(0))
            OffDamageTxt();
    }

    public void SetDamageTxt(int value, Vector3 pos)
    {//�ܺο��� �ؽ�Ʈ�� �����ϴ� �Լ�
        transform.position = pos; 
        textMeshPro.text = value.ToString();
        //������Ʈ Ȱ��ȭ�� �ִϸ��̼� �ڵ������� ��
        gameObject.SetActive(true);
    }

    void OffDamageTxt()
    {//������Ʈ �����
        gameObject.SetActive(false);
        //�ٽ� ��Ȱ���� ���� ������Ʈ ����
        GameMgr.Inst.DamageTxtEffect_P.ReturnObj(this);
    }
}
