using System;
using UnityEngine;

public interface ITimerData
{
    void Tick();
}

public enum TickType
{
    LateUpdate,
    Update,
    None
}

[Serializable]
public class TimerData : ITimerData
{
    #region Var: States
    public bool IsActive { get; private set; } = true;
    public bool IsEnded { get; private set; } = false;
    public bool IsZero => CurTime == 0;
    #endregion

    #region Var: Data
    [HideInInspector]
    public float CurTime = 0; // 타이머의 현재 시간.
    public float EndTime = 0; // 타이머의 종료 시간.
    #endregion

    #region Var: Action
    private Action OnTick;
    private Action OnEnd;
    #endregion

    #region Method: Set Up
    public TimerData SetTick(GameObject self, TickType tickType = TickType.LateUpdate)
    {
        switch (tickType)
        {
            case TickType.LateUpdate:
                TimerManager.Inst.RemoveTick_Update(self, this);
                TimerManager.Inst.AddTick_LateUpdate(self, this);
                break;
            case TickType.Update:
                TimerManager.Inst.RemoveTick_LateUpdate(self, this);
                TimerManager.Inst.AddTick_Update(self, this);
                break;
            case TickType.None:
                TimerManager.Inst.RemoveTick_Update(self, this);
                TimerManager.Inst.RemoveTick_LateUpdate(self, this);
                break;
        }

        return this;
    }
    public TimerData SetAction(Action OnTick = null, Action OnEnd = null)
    {
        this.OnTick = OnTick;
        this.OnEnd = OnEnd;

        return this;
    }
    #endregion

    #region Method: Control
    public void SetActive(bool active)
    {
        IsActive = active;
    }
    public void Restart()
    {
        CurTime = 0;
        IsEnded = false;
    }
    public void ToZero() => CurTime = 0;
    public void ToEnd() => CurTime = EndTime;
    #endregion

    void ITimerData.Tick()
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
}

public class TimerStat : ITimerData
{
    #region Var: States
    public bool IsActive { get; private set; } = true;
    public bool IsEnded { get; private set; } = false;
    public bool IsZero => CurTime == 0;
    #endregion

    #region Var: Data
    public float CurTime = 0; // 타이머의 현재 시간.
    public FloatStat EndTime; // 타이머의 종료 시간.
    #endregion

    #region Var: Action
    private Action OnTick;
    private Action OnEnd;
    #endregion

    #region Method: Set Up
    public TimerStat SetTick(GameObject self, TickType tickType = TickType.LateUpdate)
    {
        switch (tickType)
        {
            case TickType.LateUpdate:
                TimerManager.Inst.RemoveTick_Update(self, this);
                TimerManager.Inst.AddTick_LateUpdate(self, this);
                break;
            case TickType.Update:
                TimerManager.Inst.RemoveTick_LateUpdate(self, this);
                TimerManager.Inst.AddTick_Update(self, this);
                break;
            case TickType.None:
                TimerManager.Inst.RemoveTick_Update(self, this);
                TimerManager.Inst.RemoveTick_LateUpdate(self, this);
                break;
        }

        return this;
    }
    public TimerStat SetAction(Action OnTick = null, Action OnEnd = null)
    {
        this.OnTick = OnTick;
        this.OnEnd = OnEnd;

        return this;
    }
    #endregion

    #region Method: Control
    public void SetActive(bool active)
    {
        IsActive = active;
    }
    public void Restart()
    {
        CurTime = 0;
        IsEnded = false;
    }
    public void ToZero() => CurTime = 0;
    public void ToEnd() => CurTime = EndTime.Value;
    #endregion

    void ITimerData.Tick()
    {
        if (!IsActive || IsEnded)
            return;

        CurTime += Time.deltaTime;
        OnTick?.Invoke();

        if (CurTime >= EndTime.Value)
        {
            IsEnded = true;
            OnEnd?.Invoke();
            CurTime = EndTime.Value;
        }
    }
}
