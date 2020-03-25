using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class YCThreadPool : MonoBehaviour
{
    static public ConcurrentQueue<Action> Works = new ConcurrentQueue<Action>();
    static Thread t1 = null;
    static bool t1_stop;

    void OnDestroy()
    {
        t1_stop = true;
    }

    private void Awake()
    {
        if (t1 is null)
        {
            t1_stop = false;
            t1 = new Thread(() =>
            {
                Action act;
                //Debug.Log("Thread_Start");
                while (!t1_stop)
                {
                    while (!Works.IsEmpty)
                    {
                        Works.TryDequeue(out act);
                        act.Invoke();
                    }
                }
                //Debug.Log("Thread_End");
            });
            t1.Start();
        }
    }
}
