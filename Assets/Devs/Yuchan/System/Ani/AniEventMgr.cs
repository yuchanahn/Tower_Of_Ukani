using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AniEventMgr : MonoBehaviour
{
    Dictionary<string, Action> event_ = new Dictionary<string, Action>();

    public void Set(string key, Action act)
    {
        event_[key] = act;
    }

    void AniEv(string key)
    {
        if(event_.ContainsKey(key))
            event_[key]();
    }
}
