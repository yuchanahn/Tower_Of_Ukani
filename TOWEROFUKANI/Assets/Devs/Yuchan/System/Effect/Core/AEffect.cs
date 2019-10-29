using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEffect<T> : Object_ObjectPool<AEffect<T>>
{
    [SerializeField] string EndPoint;

    public override void ThisStart()
    {
        GetComponent<AniEventMgr>().Set(EndPoint, () => { DestroyObj(); });
    }
}