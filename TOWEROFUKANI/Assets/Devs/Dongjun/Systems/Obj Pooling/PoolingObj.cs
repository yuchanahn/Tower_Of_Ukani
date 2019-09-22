using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolingObj : MonoBehaviour
{
    public PoolingObj Prefab { get; private set; }

    protected virtual void OnEnable()
    {
        ResetOnActive();
    }
    protected abstract void ResetOnActive();

    public void InitPoolingObj(PoolingObj prefab)
    {
        Prefab = prefab;
    }
}