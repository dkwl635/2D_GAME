using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Stack<T> : MonoBehaviour where T : MonoBehaviour
{
	private Stack<T> poolList = new Stack<T>(); //배열
	public GameObject classObj; //프리팹
	[SerializeField] private int maxPoolSize;   //생성갯수

	//객체 생성
   private void Start()
    {//오브젝트 생성후 스택에 추가하기
		for (int i = 0; i < maxPoolSize; i++)
		{
			GameObject obj = Instantiate(classObj, transform);
			poolList.Push(obj.GetComponent<T>());
			obj.SetActive(false);
		}
	}

	public T GetObj() //사용가능한 오브젝트 순서대로 리턴
    {
		if (poolList.Count <= 1) //여분 생성
		{
			GameObject obj = Instantiate(classObj, transform);
			poolList.Push(obj.GetComponent<T>());
			obj.SetActive(false);
		}	
		return poolList.Pop();
    }
	
	public void ReturnObj(T Obj)//다시 오브젝트 풀용 스택에 넣기
    {	
		if (poolList.Count >= maxPoolSize)	
			Destroy(Obj.gameObject);
		else
			poolList.Push(Obj);
    }
		
	
}


