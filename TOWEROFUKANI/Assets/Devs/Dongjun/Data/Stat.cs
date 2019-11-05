using System;
using UnityEngine;

[Serializable]
public struct IntStat
{
    private int mod_flat;
    private float mod_percent;
    private bool needToUpdate;

    private int @base;
    private int value;

    public int Mod_Flat
    {
        get { return mod_flat; }
        set
        {
            needToUpdate = true;
            mod_flat = value;
        }
    }
    public float Mod_Percent
    {
        get { return mod_percent; }
        set
        {
            needToUpdate = true;
            mod_percent = value;
        }
    }

    public int Min 
    { get; private set; }
    public int Max 
    { get; private set; }
    public int Base
    {
        get { return @base; }
        set { @base = Mathf.Clamp(value, Min, Max); }
    }
    public int Value
    {
        get
        {
            if (needToUpdate)
            {
                needToUpdate = false;
                value = Mathf.Clamp((int)(Base * (1 + Mod_Percent * 0.01f) + 0.5f) + Mod_Flat, Min, Max);
            }
            return value;
        }
    }

    public IntStat(int @base, int? min = null, int? max = null)
    {
        mod_flat = 0;
        mod_percent = 0;
        needToUpdate = false;

        Min = min is null ? int.MinValue : min.Value;
        Max = max is null ? int.MaxValue : max.Value;
        this.@base = @base;
        value = @base;
    }
}

[Serializable]
public struct FloatStat
{
    private float mod_flat;
    private float mod_percent;
    private bool needToUpdate;

    private float @base;
    private float value;

    public float Mod_Flat
    {
        get { return mod_flat; }
        set
        {
            needToUpdate = true;
            mod_flat = value;
        }
    }
    public float Mod_Percent
    {
        get { return mod_percent; }
        set
        {
            needToUpdate = true;
            mod_percent = value;
        }
    }

    public float Min
    { get; private set; }
    public float Max
    { get; private set; }
    public float Base
    {
        get { return @base; }
        set { @base = Mathf.Clamp(value, Min, Max); }
    }
    public float Value
    {
        get
        {
            if (needToUpdate)
            {
                needToUpdate = false;
                value = Mathf.Clamp((Base * (1 + Mod_Percent * 0.01f)) + Mod_Flat, Min, Max);
            }
            return value;
        }
    }

    public FloatStat(float @base, float? min = null, float? max = null)
    {
        mod_flat = 0;
        mod_percent = 0;
        needToUpdate = false;

        Min = min is null ? float.NegativeInfinity : min.Value;
        Max = max is null ? float.PositiveInfinity : max.Value;
        this.@base = @base;
        value = @base;
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
