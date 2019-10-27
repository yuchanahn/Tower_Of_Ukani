using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_ObjectPool<T> : Object_ObjectPool_Base
{
    public static int ID = -1;

    public virtual void ThisStart()
    {
    }
    public override int GetID(){
        return ID;
    }    
    
    public override void SetID(int id)
    {
        ID = id;
    }

    public  override void SetOff()
    {
        ObjectPool.PoolingObjectDestroy(gameObject);
        gameObject.SetActive(false);
    }

    public void DestroyObj(){
        SetOff();
    }
    public void DestroyObj(float DistroyTime)
    {
        ATimer.SetAndReset(gameObject.name + GetInstanceID() , DistroyTime,SetOff);
        //StartCoroutine(DestroyObj__(DistroyTime));
    }

    public  override void SetOn(Vector2 pos)
    {
        gameObject.transform.position = pos;
        gameObject.SetActive(true);
        ThisStart();
    }

    public override void SetOnUI(Vector2 pos)
    {
        gameObject.GetComponent<RectTransform>().position = pos;
        gameObject.SetActive(true);
        ThisStart();
    }

    public static GameObject Create(Vector2 pos)
    {
        return ObjectPool.create(ID, pos);
    }

    IEnumerator DestroyObj__(float time)
    {
        yield return new WaitForSeconds(time);
        SetOff();
    }
}
