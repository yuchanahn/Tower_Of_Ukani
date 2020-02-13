using System;
using UnityEngine;

public enum TickMode
{
    Update,
    LateUpdate,
    FixedUpdate,
    Manual
}

public interface I_TimerData
{
    void Tick(float deltaTime);
}
public abstract class TimerData_Base<T> : I_TimerData 
    where T : TimerData_Base<T>
{
    private GameObject user;
    private TickMode tickMode;

    private Action OnTick;
    private Action OnEnd;

    [HideInInspector] public float CurTime = 0; // 타이머의 현재 시간.
    protected abstract float GetEndTime { get; } // 타이머의 종료 시간.

    public bool IsActive
    { get; private set; } = true;
    public bool IsEnded
    { get; private set; } = false;
    public bool IsZero => CurTime == 0;

    public T SetTick(GameObject user, TickMode tickMode = TickMode.LateUpdate)
    {
        this.user = user;
        this.tickMode = tickMode;

        TimerManager.Inst.RemoveTick_Update(user, this);
        TimerManager.Inst.RemoveTick_LateUpdate(user, this);
        TimerManager.Inst.RemoveTick_FixedUpdate(user, this);

        switch (tickMode)
        {
            case TickMode.Update:
                TimerManager.Inst.RemoveTick_LateUpdate(user, this);
                TimerManager.Inst.RemoveTick_FixedUpdate(user, this);
                TimerManager.Inst.AddTick_Update(user, this);
                break;

            case TickMode.LateUpdate:
                TimerManager.Inst.RemoveTick_Update(user, this);
                TimerManager.Inst.RemoveTick_FixedUpdate(user, this);
                TimerManager.Inst.AddTick_LateUpdate(user, this);
                break;

            case TickMode.FixedUpdate:
                TimerManager.Inst.RemoveTick_Update(user, this);
                TimerManager.Inst.RemoveTick_LateUpdate(user, this);
                TimerManager.Inst.AddTick_FixedUpdate(user, this);
                break;
        }

        return this as T;
    }
    public T SetAction(Action OnTick = null, Action OnEnd = null)
    {
        this.OnTick = OnTick;
        this.OnEnd = OnEnd;

        return this as T;
    }

    public void SetActive(bool active)
    {
        IsActive = active;

        if (tickMode == TickMode.Manual)
            return;

        if (active == false)
        {
            TimerManager.Inst.RemoveTick_Update(user, this);
            TimerManager.Inst.RemoveTick_LateUpdate(user, this);
            TimerManager.Inst.RemoveTick_FixedUpdate(user, this);
        }
        else
        {
            switch (tickMode)
            {
                case TickMode.Update:
                    TimerManager.Inst.AddTick_Update(user, this);
                    break;
                case TickMode.LateUpdate:
                    TimerManager.Inst.AddTick_LateUpdate(user, this);
                    break;
                case TickMode.FixedUpdate:
                    TimerManager.Inst.AddTick_FixedUpdate(user, this);
                    break;
            }
        }
    }
    public void Restart()
    {
        CurTime = 0;
        IsEnded = false;
    }
    public void ToZero() => CurTime = 0;
    public void ToEnd() => CurTime = GetEndTime;

    public void Tick(float deltaTime)
    {
        if (!IsActive || IsEnded)
            return;

        CurTime += deltaTime;
        OnTick?.Invoke();

        if (CurTime >= GetEndTime)
        {
            IsEnded = true;
            OnEnd?.Invoke();
            CurTime = GetEndTime;
        }
    }
}

[Serializable]
public class TimerData : TimerData_Base<TimerData>
{
    public float EndTime = 0;
    protected override float GetEndTime => EndTime;
}
public class TimerStat : TimerData_Base<TimerStat>
{
    public FloatStat EndTime;
    protected override float GetEndTime => EndTime.Value;
}
