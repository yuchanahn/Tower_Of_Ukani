using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATimer : MonoBehaviour
{
    static Dictionary<string, Action> mTimerEvent = new Dictionary<string, Action>();
    static Dictionary<string, (float,float)> mTimer = new Dictionary<string, (float, float)>();


    public static void Set(string key, float t, Action ev)
    {
        if(mTimerEvent.ContainsKey(key))    mTimerEvent[key] += ev;
        else                                mTimerEvent[key] = ev;
        mTimer[key] = (t, 0);
    }
    public static void Pop(string key)
    {
        mTimerEvent.Remove(key);
    }


    void Update()
    {
        foreach(var i in mTimerEvent)
        {
            mTimer[i.Key] = (mTimer[i.Key].Item1, mTimer[i.Key].Item2+Time.deltaTime);
            if (mTimer[i.Key].Item1 < mTimer[i.Key].Item2)
            {
                i.Value();
                mTimer[i.Key] = (mTimer[i.Key].Item1, 0);
            }
        }
    }
}
