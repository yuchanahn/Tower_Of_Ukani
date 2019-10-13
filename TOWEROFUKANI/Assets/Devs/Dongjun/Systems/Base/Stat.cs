using System;
using UnityEngine;

[Serializable]
public struct IntStat
{
    public int Base;
    public int Value => (int)(Base * (1 + (percentBonus / 100)) + flatBonus + 0.5f);

    [HideInInspector] public float percentBonus;
    [HideInInspector] public float flatBonus;
}

[Serializable]
public struct FloatStat
{
    public float Base;
    public float Value => Base * (1 + (percentBonus / 100)) + flatBonus;

    [HideInInspector] public float percentBonus;
    [HideInInspector] public float flatBonus;
}

[Serializable]
public class TimerStat : ITimerData
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
    public FloatStat EndTime; // 타이머릐 최대 시간.
    #endregion

    #region Var: Action
    private Action OnTick;
    private Action OnEnd;
    #endregion


    public void Init(GameObject self, Action OnTick = null, Action OnEnd = null)
    {
        // Init Timer
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

        if (CurTime >= EndTime.Value)
        {
            IsEnded = true;
            OnEnd?.Invoke();
            CurTime = EndTime.Value;
        }
    }
    public void Restart()
    {
        CurTime = 0;
        IsEnded = false;
    }

    public void ToZero() => CurTime = 0;
    public void ToEnd() => CurTime = EndTime.Value;
}
