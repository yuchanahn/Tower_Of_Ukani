using System;
using UnityEngine;

public enum StatusType
{
    Buff,
    Debuff
}

public sealed class StatusID { }
public abstract class PlayerStatusEffect
{
    private Action onStart;
    private Action onEnd;

    public StatusID ID
    { get; protected set; }
    public GameObject Caster
    { get; protected set; }
    public StatusType StatusType
    { get; protected set; }
    public TimerData Timer
    { get; protected set; } = null;

    public PlayerStatusEffect(
        StatusID id,
        GameObject caster,
        StatusType statusType,
        float endTime = 0,
        Action onStart = null,
        Action onEnd = null)
    {
        ID = id;
        Caster = caster;
        StatusType = statusType;
        this.onStart = onStart;
        this.onEnd = onEnd;

        if (endTime < 0)
        {
            Timer = new TimerData();
            Timer.
                SetTick(PlayerStatus.Inst.gameObject).
                SetAction(OnEnd: () => PlayerStatus.RemoveEffect(this)).
                EndTime = endTime;
        }
    }

    public virtual void OnStart()
    {
        onStart?.Invoke();
    }
    public virtual void OnEnd()
    {
        onEnd?.Invoke();
    }
}

