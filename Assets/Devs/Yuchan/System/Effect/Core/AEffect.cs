using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEffect<T> : Object_ObjectPool<AEffect<T>>
{
    [SerializeField] string EndPoint;

    public override void Begin()
    {
        GetComponent<AniEventMgr>().Set(EndPoint, () => { DestroyObj(); });
    }
}