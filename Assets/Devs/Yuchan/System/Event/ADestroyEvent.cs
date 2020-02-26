using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ADestroyEvent : MonoBehaviour
{
    Dictionary<string, Action> DestroyEvents = new Dictionary<string, Action>();

    public void Add(string k, Action Ev)
    {
        DestroyEvents[k] = Ev;
    }

    public void Remove(string k)
    {
        DestroyEvents.Remove(k);
    }

    private void OnDestroy()
    {
        foreach (var i in DestroyEvents)
        {
            i.Value();
        }
    }

    static void Add(Component ID, String str, GameObject target, Action ev)
    {
        if (target is null) return;
        if (target.GetComponent<ADestroyEvent>() is null)
        {
            target.AddComponent<ADestroyEvent>();
        }
        target.GetComponent<ADestroyEvent>().Add(ID.GetInstanceID() + str, ev);
    }

    static void Remove(Component ID, String str, GameObject target)
    {
        if (target is null) return;
        if (target.GetComponent<ADestroyEvent>() is null) return;
        var key = ID.GetInstanceID() + str;
        if (target.GetComponent<ADestroyEvent>().DestroyEvents.ContainsKey(key))
        {
            target.GetComponent<ADestroyEvent>().DestroyEvents.Remove(key);
        }
    }
}
