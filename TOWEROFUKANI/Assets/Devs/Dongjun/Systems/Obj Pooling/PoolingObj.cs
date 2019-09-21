using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolingObj : MonoBehaviour
{
    [HideInInspector]
    public PoolingObj prefab;

    protected virtual void OnEnable()
    {
        ResetOnActive();
    }
    protected abstract void ResetOnActive();
}
