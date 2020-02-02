using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OBB_State<T> : MonoBehaviour where T : OBB_Data
{
    protected T data { get; private set; }

    public void InitData(T data)
    {
        this.data = data;
        Init();
    }
    public virtual void Init() { }

    public virtual void OnEnter() { }
    public virtual void OnLateEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnFixedUpdate() { }
}
