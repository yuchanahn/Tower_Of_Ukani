using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimerManager : SingletonBase<TimerManager>
{
    private Dictionary<GameObject, List<ITimerData>> lateUpdateTimers = new Dictionary<GameObject, List<ITimerData>>();
    private Dictionary<GameObject, List<ITimerData>> updateTimers = new Dictionary<GameObject, List<ITimerData>>();
    private GameObject curTickingObj_LateUpdate;
    private GameObject curTickingObj_Update;

    private void Update()
    {
        TickTimers(updateTimers, curTickingObj_Update);
    }
    private void LateUpdate()
    {
        TickTimers(lateUpdateTimers, curTickingObj_LateUpdate);
    }

    public void TickTimers(Dictionary<GameObject, List<ITimerData>> timers, GameObject curTickingObj)
    {
        if (timers.Count == 0)
            return;

        for (int i = timers.Count - 1; i >= 0; i--)
        {
            curTickingObj = timers.ElementAt(i).Key;

            // Check Game Object
            if (curTickingObj == null)
            {
                timers.Remove(curTickingObj_Update);
                continue;
            }

            // Run Timer
            for (int j = timers[curTickingObj].Count - 1; j >= 0; j--)
                timers[curTickingObj][j].Tick();
        }
    }

    public void AddToLateUpdate(GameObject go, ITimerData data)
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
    public void RemoveFromLateUpdate(GameObject go, ITimerData data)
    {
        if (!lateUpdateTimers.ContainsKey(go) || !lateUpdateTimers[go].Contains(data))
            return;

        lateUpdateTimers[go].Remove(data);
    }

    public void AddToUpdate(GameObject go, ITimerData data)
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
    public void RemoveFromUpdate(GameObject go, ITimerData data)
    {
        if (!updateTimers.ContainsKey(go) || !updateTimers[go].Contains(data))
            return;

        updateTimers[go].Remove(data);
    }
}
