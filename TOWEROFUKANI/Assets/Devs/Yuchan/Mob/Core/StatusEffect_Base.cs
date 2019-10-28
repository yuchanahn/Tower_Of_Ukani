using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Base : MonoBehaviour
{
    [SerializeField] protected int priority;

    protected virtual void Start()
    {
    }

    public virtual void SetValue<T>(SE_Stat<T> value, T data)
    {
        value.SetValue(priority, data);
    }

    protected virtual void OnSEDestroy()
    {
        //GetComponent<StatusEffect_Object>()?.Remove(this);
    }
}
