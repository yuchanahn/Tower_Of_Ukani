using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATimer : MonoBehaviour
{
    static Dictionary<string, Action> mTimerEvent = new Dictionary<string, Action>();
    static Dictionary<string, (float, float)> mTimer = new Dictionary<string, (float, float)>();

    static Dictionary<object, (float, float, Action)> mT = new Dictionary<object, (float, float, Action)>();

    public static void Set(string key, float t, Action ev)
    {
        if (mTimerEvent.ContainsKey(key)) mTimerEvent[key] += ev;
        else mTimerEvent[key] = ev;
        mTimer[key] = (t, 0);

    }

    public static void SetAndReset(string key, float t, Action ev)
    {
        if (mTimerEvent.ContainsKey(key)) mTimerEvent[key] = ev;
        else mTimerEvent[key] = ev;
        mTimer[key] = (t, 0);
    }
    public static void Pop(string key)
    {
        mTimerEvent.Remove(key);
        mTimer.Remove(key);
    }

    public static void Set(object key, float t, Action ev)
    {
        mT[key] = (t, 0, ev);
    }
    public static void Tick(object key)
    {
        if (!mT.ContainsKey(key)) { print("TICK NO KEY!"); return; }
        if (mT[key].Item1 < mT[key].Item2 + Time.deltaTime)
        {
            var act = mT[key].Item3;
            mT.Remove(key);
            act();
            return;
        }

        mT[key] = (mT[key].Item1, mT[key].Item2 + Time.deltaTime, mT[key].Item3);
    }

    void Update()
    {
        Queue<string> keys = new Queue<string>();
        foreach (var i in mTimerEvent)
        {
            mTimer[i.Key] = (mTimer[i.Key].Item1, mTimer[i.Key].Item2 + Time.deltaTime);
            if (mTimer[i.Key].Item1 < mTimer[i.Key].Item2)
            {
                i.Value();
                keys.Enqueue(i.Key);
            }
        }
        while (keys.Count > 0) Pop(keys.Dequeue());
    }
}
