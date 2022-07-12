using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Stack<T> : MonoBehaviour
{
    private Stack<T> poolList = new Stack<T>(); //�迭
    private GameObject classObj;	//������
    private int maxPoolSize = 10;	//����
    private Transform objTr;	//������Ʈ ��ȯ�ϰ� ��Ƶ� 


	public ObjectPool_Stack(GameObject classObj, int maxPoolSize, Transform objTr)
	{
		this.classObj = classObj;
		this.maxPoolSize = maxPoolSize;
		this.objTr = objTr;

        for (int i = 0; i < maxPoolSize; i++)
        {
			poolList.Push(Instantiate(classObj, objTr).GetComponent<T>());
        }
	}
	//��ü ����
	
	public T GetObj()
    {
		if (poolList.Count <= 0)
			return (Instantiate(classObj, objTr).GetComponent<T>());

		return poolList.Pop();
    }
	
	public void ReturnObj(T Obj)
    {
		poolList.Push(Obj);
    }
		
	
}
