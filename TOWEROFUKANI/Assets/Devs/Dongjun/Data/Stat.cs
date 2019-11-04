using System;
using UnityEngine;

[Serializable]
public struct IntStat
{
    // Default
    public int Base;
    public int Min;
    public int Max;

    // Get Value
    public int Value => Mathf.Clamp((int)((Base * (1 + (Mod_Percent * 0.01f))) + Mod_Flat + 0.5f), Min, Max);

    // Bonus
    [HideInInspector] public float Mod_Percent;
    [HideInInspector] public int Mod_Flat;

    public IntStat(int _base, int? min = null, int? max = null)
    {
        Base = _base;
        Min = min is null ? int.MinValue : min.Value;
        Max = max is null ? int.MaxValue : max.Value;
        Mod_Percent = 0f;
        Mod_Flat = 0;
    }
}

[Serializable]
public struct FloatStat
{
    // Default
    public float Base;
    public float Min;
    public float Max;

    // Get Value
    public float Value => Mathf.Clamp((Base * (1 + (Mod_Percent * 0.01f))) + Mod_Flat, Min, Max);

    // Bonus
    [HideInInspector] public float Mod_Percent;
    [HideInInspector] public float Mod_Flat;

    public FloatStat(float _base, float? min = null, float? max = null)
    {
        Base = _base;
        Min = min is null ? float.NegativeInfinity : min.Value;
        Max = max is null ? float.PositiveInfinity : max.Value;
        Mod_Percent = 0f;
        Mod_Flat = 0f;
    }
}

[Serializable]
public class TimerStat : ITimerData
{
    #region Var: States
    public bool StartAsEnded = false;
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
