using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Base : MonoBehaviour
{
    [SerializeField] protected int priority;
    protected StatusEffect_Object SE_Obj;
    protected YCTimerData destroy_timer = null;
    protected virtual void Start()
    {
        SE_Obj = GetComponent<StatusEffect_Object>();
    }

    public virtual void SetValue<T>(SE_Stat<T> value, T data)
    {
        value.SetValue(priority, data);
    }

    public virtual void SetValue<T>(SE_Stat<T> value, T data, int priority)
    {
        value.SetValue(priority, data);
    }

    private void OnDestroy()
    {
        if(SE_Obj) OnSEDestroy();
    }
    protected virtual void OnSEDestroy()
    {
        //GetComponent<StatusEffect_Object>()?.Remove(this);
    }
}
