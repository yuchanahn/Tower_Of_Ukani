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
    GameObject User { get; }
    void Tick(float deltaTime);
}
public abstract class TimerData_Base<T> : I_TimerData 
    where T : TimerData_Base<T>
{
    private TickMode tickMode;

    private Action onStart;
    private Action onTick;
    private Action onEnd;

    [HideInInspector] public float CurTime = 0; // 타이머의 현재 시간.
    protected abstract float GetEndTime { get; } // 타이머의 종료 시간.

    public GameObject User { get; private set; }
    public bool IsActive
    { get; private set; } = true;
    public bool IsEnded => CurTime == GetEndTime;
    public bool IsZero => CurTime == 0;

    public T SetTick(GameObject user, TickMode tickMode = TickMode.LateUpdate)
    {
        User = user;
        this.tickMode = tickMode;

        TimerManager.Inst.RemoveTimer(this, TickMode.Update);
        TimerManager.Inst.RemoveTimer(this, TickMode.LateUpdate);
        TimerManager.Inst.RemoveTimer(this, TickMode.FixedUpdate);
        TimerManager.Inst.AddTimer(this, tickMode);

        return this as T;
    }
    public T SetAction(Action onStart = null, Action onTick = null, Action onEnd = null)
    {
        this.onStart = onStart;
        this.onTick = onTick;
        this.onEnd = onEnd;

        return this as T;
    }

    public void SetActive(bool active)
    {
        IsActive = active;

        if (tickMode == TickMode.Manual)
            return;

        if (active == false)
        {
            TimerManager.Inst.RemoveTimer(this, TickMode.Update);
            TimerManager.Inst.RemoveTimer(this, TickMode.LateUpdate);
            TimerManager.Inst.RemoveTimer(this, TickMode.FixedUpdate);
        }
        else
        {
            TimerManager.Inst.AddTimer(this, tickMode);
        }
    }
    public void Reset() => CurTime = 0;
    public void ToEnd() => CurTime = GetEndTime;

    public void Tick(float deltaTime)
    {
        if (!IsActive || IsEnded)
            return;

        if (CurTime == 0)
            onStart?.Invoke();

        CurTime += deltaTime;
        onTick?.Invoke();

        if (CurTime >= GetEndTime)
        {
            CurTime = GetEndTime;
            onEnd?.Invoke();
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
