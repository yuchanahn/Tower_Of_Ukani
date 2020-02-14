using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct PoolingObject
{
    public GameObject Object;
    public int Count;
}

public class ObjectPool : MonoBehaviour
{
    static ObjectPool Inst;

    [SerializeField] PoolingObject[] poolingObjs;
    [SerializeField] Transform UI;

    Queue<GameObject>[] objects;

    void Awake()
    {
        Inst = this;

        objects = new Queue<GameObject>[poolingObjs.Length];
        for (int i = 0; i < poolingObjs.Length; i++)
        {
            objects[i] = new Queue<GameObject>();
            CreateObj(i);
        }
    }


    void CreateObj(int id)
    {
        for (int j = 0; j < poolingObjs[id].Count; j++)
        {
            GameObject GObj = Instantiate(poolingObjs[id].Object, transform);
            GObj.name = poolingObjs[id].Object.name;
            Object_ObjectPool_Base Pool_Base = GObj.GetComponent<Object_ObjectPool_Base>();
            Pool_Base.SetID(id);
            Pool_Base.SetOff();
        }
    }

    public static GameObject create(int id, Vector2 pos)
    {
        if (Inst.objects[id].Count == 0)
        { 
            Inst.CreateObj(id);
        }

        GameObject GObj = Inst.objects[id].Dequeue();
        GObj.GetComponent<Object_ObjectPool_Base>().SetOn(pos);
        return GObj;
    }

    public static GameObject createUI(int id, Vector2 pos)
    {
        if (Inst.objects[id].Count == 0)
        { Inst.CreateObj(id); }
        
        GameObject GObj = Inst.objects[id].Dequeue();
        GObj.transform.SetParent(Inst.UI, false);
        GObj.GetComponent<Object_ObjectPool_Base>().SetOnUI(pos);


        return GObj;
    }

    public static void PoolingObjectDestroy(GameObject obj)
    {
        Inst.objects[obj.GetComponent<Object_ObjectPool_Base>().GetID()].Enqueue(obj);
    }
}
