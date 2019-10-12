using System;
using UnityEngine;

[Serializable]
public struct IntStat
{
    public int Base;
    [HideInInspector] public int Cur;

    [HideInInspector] public float percentAdd;
    [HideInInspector] public float flatAdd;

    public void Init()
    {
        Cur = Base;

        percentAdd = 0;
        flatAdd = 0;
    }

    public void ApplyAddedStat()
    {
        Cur = (int)(Base * (1 + (percentAdd / 100)) + flatAdd + 0.5f);
    }
}

[Serializable]
public struct FloatStat
{
    public float Base;
    [HideInInspector] public float Cur;

    [HideInInspector] public float percentAdd;
    [HideInInspector] public float flatAdd;

    public void Init()
    {
        Cur = Base;

        percentAdd = 0;
        flatAdd = 0;
    }

    public void ApplyAddedStat()
    {
        Cur = Base * (1 + (percentAdd / 100)) + flatAdd;
    }
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

        if (CurTime >= EndTime.Cur)
        {
            IsEnded = true;
            OnEnd?.Invoke();
            CurTime = EndTime.Cur;
        }
    }
    public void Restart()
    {
        CurTime = 0;
        IsEnded = false;
    }

    public void ToZero() => CurTime = 0;
    public void ToEnd() => CurTime = EndTime.Cur;
}
