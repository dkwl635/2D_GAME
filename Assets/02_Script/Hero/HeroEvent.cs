using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroEvent : MonoBehaviour
{
    //�ִϸ��̼� �̺�Ʈ�� �����ϱ� ���� HeroEvent
    public HeroCtrl HeroCtrl;
    public void Attack_Event() //�����ϴ� �̺�Ʈ �Լ�
    {
        HeroCtrl.Attack_Event();
    }
}
