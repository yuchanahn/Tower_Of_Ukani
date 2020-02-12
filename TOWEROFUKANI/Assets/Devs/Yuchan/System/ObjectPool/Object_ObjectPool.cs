using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_ObjectPool<T> : Object_ObjectPool_Base
{
    public static int ID = -1;

    public virtual void Begin()
    {
    }
    public override int GetID(){
        return ID;
    }    
    
    public override void SetID(int id)
    {
        ID = id;
    }

    public override void SetOff()
    {
        End();
        ObjectPool.PoolingObjectDestroy(gameObject);
        gameObject.SetActive(false);
    }


    public virtual void End()
    {
    }

    public void DestroyObj()
    {
        ATimer.Pop(GetInstanceID() + "DestroyObj");
        SetOff();
    }

    void OnDestroy()
    {
        ATimer.Pop(gameObject.name + GetInstanceID());
    }

    public void DestroyObj(float DistroyTime)
    {
        ATimer.SetAndReset(GetInstanceID() + "DestroyObj", DistroyTime, SetOff);
    }

    public  override void SetOn(Vector2 pos)
    {
        gameObject.transform.position = pos;
        gameObject.SetActive(true);
        Begin();
    }

    public override void SetOnUI(Vector2 pos)
    {
        (gameObject.transform as RectTransform).position = pos;
        gameObject.SetActive(true);
        Begin();
    }

    public static GameObject Create(Vector2 pos)
    {
        return ObjectPool.create(ID, pos);
    }

    public static GameObject CreateUI(Vector2 pos)
    {
        return ObjectPool.createUI(ID, pos);
    }
}
