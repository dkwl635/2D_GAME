using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMgr : MonoBehaviour
{
    static public GameMgr Inst;

    [Header("ObjPool")]
    public GameObject damageTxt_Prefab;
    public Transform damageTxtTr; 

    //DamageTxt
    public Stack<DamageTxt> DamageTxt_Pool = new Stack<DamageTxt>();

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        Create_ObjPool();
    }

    void Create_ObjPool()
    {
        for (int i = 0; i < 20; i++)
        {
            DamageTxt_Pool.Push(Instantiate(damageTxt_Prefab, damageTxtTr).GetComponent<DamageTxt>());
        }
    }

     public void Pop_DamageTxt(int value, Vector3 pos)
    {
        DamageTxt obj;
        if (DamageTxt_Pool.Count <= 0)
            obj = Instantiate(damageTxt_Prefab, damageTxtTr).GetComponent<DamageTxt>();
        else
            obj = DamageTxt_Pool.Pop();

        obj.SetDamageTxt(value, pos);       
    }

    public void PushBack_DamageTxt(DamageTxt obj)
    {
        DamageTxt_Pool.Push(obj);
    }

}
