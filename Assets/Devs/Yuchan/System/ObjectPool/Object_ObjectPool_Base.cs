using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_ObjectPool_Base : MonoBehaviour, IObjectPool
{
    public virtual GameObject CreateThis(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }

    public virtual int GetID(){
        return -1;
    }

    public virtual void SetID(int id)
    {
        
    }


    public  virtual  void SetOff()
    {
    }

    public virtual void SetOn(Vector2 pos)
    {
    }

    public virtual void SetOnUI(Vector2 pos)
    {
    }

    public virtual void Init(Vector2 pos)
    {
    }
}
