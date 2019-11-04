using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ITimerData
{
    void Tick();
}

[Serializable]
public class TimerData : ITimerData
{
    #region Var: States
    [SerializeField]
    protected bool StartAsEnded = false;
    public bool IsActive { get; private set; } = true;
    public bool IsEnded { get; private set; } = false;
    public bool IsZero => CurTime == 0;
    #endregion

    #region Var: Data
    [HideInInspector]
    public float CurTime = 0; // 타이머의 현재 시간.
    public float EndTime = 0; // 타이머릐 최대 시간.
    #endregion

    #region Var: Action
    private Action OnTick;
    private Action OnEnd;
    #endregion


    public void Init(GameObject self, Action OnTick = null, Action OnEnd = null)
    {
        TimerManager.Inst.AddTimer(self, this);

        this.OnTick = OnTick;
        this.OnEnd = OnEnd;

        if (StartAsEnded)
            ToEnd();
    }
    public void SetAction(Action OnTick = null, Action OnEnd = null)
    {
        this.OnTick = OnTick;
        this.OnEnd = OnEnd;
    }

    public void SetActive(bool active)
    {
        IsActive = active;
    }

    public void UseAutoTick(GameObject self, bool use)
    {
        if (use)
            TimerManager.Inst.AddTimer(self, this);
        else
            TimerManager.Inst.RemoveTimer(self, this);
    }
    public void Tick()
    {
        if (!IsActive || IsEnded)
            return;

        CurTime += Time.deltaTime;
        OnTick?.Invoke();

        if (CurTime >= EndTime)
        {
            IsEnded = true;
            OnEnd?.Invoke();
            CurTime = EndTime;
        }
    }
    public void Restart()
    {
        CurTime = 0;
        IsEnded = false;
    }

    public void ToZero() => CurTime = 0;
    public void ToEnd() => CurTime = EndTime;
}

public class TimerManager : SingletonBase<TimerManager>
{
    private Dictionary<GameObject, List<ITimerData>> timers = new Dictionary<GameObject, List<ITimerData>>();
    private GameObject curTickingObj;

    private void LateUpdate()
    {
        TickTimers();
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
    public void TickTimers()
    {
        if (timers.Count == 0)
            return;

        for (int i = timers.Count - 1; i >= 0; i--)
        {
            curTickingObj = timers.ElementAt(i).Key;

            if (curTickingObj is null)
            {
                timers.Remove(curTickingObj);
                continue;
            }

            for (int j = timers[curTickingObj].Count - 1; j >= 0; j--)
                timers[curTickingObj][j].Tick();
        }
    }
}
