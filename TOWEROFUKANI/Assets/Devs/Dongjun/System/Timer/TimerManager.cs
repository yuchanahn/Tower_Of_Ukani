using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimerManager : SingletonBase<TimerManager>
{
    private Dictionary<GameObject, List<I_TimerData>> timers_Update = new Dictionary<GameObject, List<I_TimerData>>();
    private Dictionary<GameObject, List<I_TimerData>> timers_LateUpdate = new Dictionary<GameObject, List<I_TimerData>>();
    private Dictionary<GameObject, List<I_TimerData>> timers_FixedUpdate = new Dictionary<GameObject, List<I_TimerData>>();

    private void Update()
    {
        TickTimers(timers_Update);
    }
    private void LateUpdate()
    {
        TickTimers(timers_LateUpdate);
    }
    private void FixedUpdate()
    {
        TickTimers(timers_FixedUpdate);
    }

    public void TickTimers(Dictionary<GameObject, List<I_TimerData>> timers)
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
            {
                if (timers[curTickingObj][j] == null)
                {
                    timers[curTickingObj].RemoveAt(j);
                    continue;
                }
                timers[curTickingObj][j].Tick(Time.deltaTime);
            }
        }
    }

    // Update
    public void AddTick_Update(GameObject go, I_TimerData data)
    {
        if (timers_Update.ContainsKey(go) && timers_Update[go].Contains(data))
            return;

        if (timers_Update.ContainsKey(go))
        {
            timers_Update[go].Add(data);
            return;
        }

        if (!timers_Update.ContainsKey(go))
            timers_Update.Add(go, new List<I_TimerData>());

        timers_Update[go].Add(data);
    }
    public void RemoveTick_Update(GameObject go, I_TimerData data)
    {
        if (!timers_Update.ContainsKey(go) || !timers_Update[go].Contains(data))
            return;

        timers_Update[go].Remove(data);
    }

    // LateUpdate
    public void AddTick_LateUpdate(GameObject go, I_TimerData data)
    {
        if (timers_LateUpdate.ContainsKey(go) && timers_LateUpdate[go].Contains(data))
            return;

        if (timers_LateUpdate.ContainsKey(go))
        {
            timers_LateUpdate[go].Add(data);
            return;
        }

        if (!timers_LateUpdate.ContainsKey(go))
            timers_LateUpdate.Add(go, new List<I_TimerData>());

        timers_LateUpdate[go].Add(data);
    }
    public void RemoveTick_LateUpdate(GameObject go, I_TimerData data)
    {
        if (!timers_LateUpdate.ContainsKey(go) || !timers_LateUpdate[go].Contains(data))
            return;

        timers_LateUpdate[go].Remove(data);
    }

    // FixedUpdate
    public void AddTick_FixedUpdate(GameObject go, I_TimerData data)
    {
        if (timers_Update.ContainsKey(go) && timers_Update[go].Contains(data))
            return;

        if (timers_Update.ContainsKey(go))
        {
            timers_Update[go].Add(data);
            return;
        }

        if (!timers_Update.ContainsKey(go))
            timers_Update.Add(go, new List<I_TimerData>());

        timers_Update[go].Add(data);
    }
    public void RemoveTick_FixedUpdate(GameObject go, I_TimerData data)
    {
        if (!timers_Update.ContainsKey(go) || !timers_Update[go].Contains(data))
            return;

        timers_Update[go].Remove(data);
    }
}
