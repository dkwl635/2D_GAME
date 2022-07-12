using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameMgr : MonoBehaviour
{
    static public GameMgr Inst;

    [Header("ObjPool")]
    public GameObject damageTxt_Prefab;
    public Transform damageTxtTr;

    public ObjectPool_Stack<DamageTxt> DamageTxt_Stack;


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
        DamageTxt_Stack = new ObjectPool_Stack<DamageTxt>(damageTxt_Prefab, 10, damageTxtTr); 
    }


}
