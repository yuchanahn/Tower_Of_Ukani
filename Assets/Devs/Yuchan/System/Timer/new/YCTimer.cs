using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class YCTimerData
{
    public float time;
    public float timer_t;

    public int layer_f;
    public Action event_;
    public YCTimerData(float t, Action ev, int f = 0)
    {
        time = t;
        layer_f = f;
        event_ = ev;
        timer_t = 0f;
    }
}


public class YCTimer : MonoBehaviour
{
    static List<YCTimerData> timers = new List<YCTimerData>();
    public static void Add(YCTimerData t){
        t.timer_t = 0f;
        timers.Add(t);
    }
    public static void Remove(YCTimerData t) => timers.Remove(t);


    private void Update()
    {
        timers.ForEach(x => x.timer_t += Time.deltaTime);
        timers.Where(x => x.timer_t >= x.time)
              .ToList()
              .ForEach(x => { x.event_(); timers.Remove(x); });
    }

    private void OnDestroy()
    {
        timers.ToList().ForEach(x => timers.Remove(x));
    }
}