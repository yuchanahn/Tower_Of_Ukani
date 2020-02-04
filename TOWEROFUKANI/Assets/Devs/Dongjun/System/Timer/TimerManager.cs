using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimerManager : SingletonBase<TimerManager>
{
    private Dictionary<GameObject, List<ITimerData>> lateUpdateTimers = new Dictionary<GameObject, List<ITimerData>>();
    private Dictionary<GameObject, List<ITimerData>> updateTimers = new Dictionary<GameObject, List<ITimerData>>();

    private void Update()
    {
        TickTimers(updateTimers);
    }
    private void LateUpdate()
    {
        TickTimers(lateUpdateTimers);
    }

    public void TickTimers(Dictionary<GameObject, List<ITimerData>> timers)
    {
        if (timers.Count == 0)
            return;

        GameObject curTickingObj;

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

    public void AddTick_LateUpdate(GameObject go, ITimerData data)
    {
        if (lateUpdateTimers.ContainsKey(go) && lateUpdateTimers[go].Contains(data))
            return;

        if (lateUpdateTimers.ContainsKey(go))
        {
            lateUpdateTimers[go].Add(data);
            return;
        }

        if (!lateUpdateTimers.ContainsKey(go))
            lateUpdateTimers.Add(go, new List<ITimerData>());

        lateUpdateTimers[go].Add(data);
    }
    public void RemoveTick_LateUpdate(GameObject go, ITimerData data)
    {
        if (!lateUpdateTimers.ContainsKey(go) || !lateUpdateTimers[go].Contains(data))
            return;

        lateUpdateTimers[go].Remove(data);
    }

    public void AddTick_Update(GameObject go, ITimerData data)
    {
        if (updateTimers.ContainsKey(go) && updateTimers[go].Contains(data))
            return;

        if (updateTimers.ContainsKey(go))
        {
            updateTimers[go].Add(data);
            return;
        }

        if (!updateTimers.ContainsKey(go))
            updateTimers.Add(go, new List<ITimerData>());

        updateTimers[go].Add(data);
    }
    public void RemoveTick_Update(GameObject go, ITimerData data)
    {
        if (!updateTimers.ContainsKey(go) || !updateTimers[go].Contains(data))
            return;

        updateTimers[go].Remove(data);
    }
}
