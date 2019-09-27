using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolingObj : MonoBehaviour
{
    public PoolingObj Prefab { get; private set; }

    public abstract void ResetOnActive();

    public void InitPoolingObj(PoolingObj prefab)
    {
        Prefab = prefab;
    }
}