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
    public void SetTick(GameObject self, TickType tickType = TickType.LateUpdate)
    {
        switch (tickType)
        {
            case TickType.LateUpdate:
                TimerManager.Inst.RemoveFromUpdate(self, this);
                TimerManager.Inst.AddToLateUpdate(self, this);
                break;
            case TickType.Update:
                TimerManager.Inst.RemoveFromLateUpdate(self, this);
                TimerManager.Inst.AddToUpdate(self, this);
                break;
            default:
                TimerManager.Inst.RemoveFromUpdate(self, this);
                TimerManager.Inst.RemoveFromLateUpdate(self, this);
                break;
        }
    }
    public void SetAction(Action OnTick = null, Action OnEnd = null)
    {
        this.OnTick = OnTick;
        this.OnEnd = OnEnd;
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
    public void SetTick(GameObject self, TickType tickType = TickType.LateUpdate)
    {
        switch (tickType)
        {
            case TickType.LateUpdate:
                TimerManager.Inst.RemoveFromUpdate(self, this);
                TimerManager.Inst.AddToLateUpdate(self, this);
                break;
            case TickType.Update:
                TimerManager.Inst.RemoveFromLateUpdate(self, this);
                TimerManager.Inst.AddToUpdate(self, this);
                break;
            default:
                TimerManager.Inst.RemoveFromUpdate(self, this);
                TimerManager.Inst.RemoveFromLateUpdate(self, this);
                break;
        }
    }
    public void SetAction(Action OnTick = null, Action OnEnd = null)
    {
        this.OnTick = OnTick;
        this.OnEnd = OnEnd;
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
