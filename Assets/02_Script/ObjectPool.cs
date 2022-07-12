using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Stack<T> : MonoBehaviour
{
    private Stack<T> poolList = new Stack<T>(); //배열
    private GameObject classObj;	//프리팹
    private int maxPoolSize = 10;	//갯수
    private Transform objTr;	//오브젝트 소환하고 모아둘 


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
	//객체 생성
	
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
