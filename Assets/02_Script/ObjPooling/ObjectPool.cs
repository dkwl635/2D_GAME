using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Stack<T> : MonoBehaviour where T : MonoBehaviour
{
	private Stack<T> poolList = new Stack<T>(); //硅凯
	public GameObject classObj; //橇府普
	private int maxPoolSize = 10;   //肮荐

	//按眉 积己
   private void Start()
    {
		for (int i = 0; i < maxPoolSize; i++)
		{
			GameObject obj = Instantiate(classObj, transform);
			poolList.Push(obj.GetComponent<T>());
			obj.SetActive(false);
		}
	}

	public T GetObj()
    {
		if (poolList.Count <= 1) //咯盒 积己
		{
			GameObject obj = Instantiate(classObj, transform);
			poolList.Push(obj.GetComponent<T>());
			obj.SetActive(false);
		}
		
		return poolList.Pop();
    }
	
	public void ReturnObj(T Obj)
    {	
		if (poolList.Count >= maxPoolSize)	
			Destroy(Obj.gameObject);
		else
			poolList.Push(Obj);
    }
		
	
}
