using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimerManager : SingletonBase<TimerManager>
{
    private Dictionary<GameObject, List<ITimerData>> timers = new Dictionary<GameObject, List<ITimerData>>();
    private GameObject curTickingObj;

    private void LateUpdate()
    {
        TickTimers();
    }

    public void TickTimers()
    {
        if (timers.Count == 0)
            return;

        for (int i = timers.Count - 1; i >= 0; i--)
        {
            curTickingObj = timers.ElementAt(i).Key;

            // Check Game Object
            if (curTickingObj == null)
            {
                timers.Remove(curTickingObj);
                continue;
            }

            // Run Timer
            for (int j = timers[curTickingObj].Count - 1; j >= 0; j--)
                timers[curTickingObj][j].Tick();
        }
    }

    public void AddTimer(GameObject go, ITimerData data)
    {
        if (timers.ContainsKey(go))
        {
            timers[go].Add(data);
            return;
        }

        if (timers.ContainsKey(go) && timers[go].Contains(data))
            return;

        if (!timers.ContainsKey(go))
            timers.Add(go, new List<ITimerData>());

        timers[go].Add(data);
    }
    public void RemoveTimer(GameObject go, ITimerData data)
    {
        if (!timers.ContainsKey(go) || !timers[go].Contains(data))
            return;

        timers[go].Remove(data);
    }
}
