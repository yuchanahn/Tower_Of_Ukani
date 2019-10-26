using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_ObjectPool_Base : MonoBehaviour
{
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
}
