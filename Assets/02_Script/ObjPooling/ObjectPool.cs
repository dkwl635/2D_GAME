using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Stack<T> : MonoBehaviour where T : MonoBehaviour
{
	private Stack<T> poolList = new Stack<T>(); //�迭
	public GameObject classObj; //������
	private int maxPoolSize = 10;   //����

	//��ü ����
   private void Start()
    {
		for (int i = 0; i < maxPoolSize; i++)
		{
			poolList.Push(Instantiate(classObj, transform).GetComponent<T>());
		}
	}

	public T GetObj()
    {
		if (poolList.Count <= 0)
			return Instantiate(classObj, transform).GetComponent<T>();

		return poolList.Pop();
    }
	
	public void ReturnObj(T Obj)
    {
		
		if (poolList.Count >= maxPoolSize)
		{
			Destroy(Obj.gameObject);
		}
		else
			poolList.Push(Obj);
    }
		
	
}
