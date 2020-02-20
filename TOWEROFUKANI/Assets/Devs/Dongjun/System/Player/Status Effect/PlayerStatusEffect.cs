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
        float endTime = 0)
    {
        ID = id;
        Caster = caster;
        StatusType = statusType;

        if (endTime < 0)
        {
            if (Timer == null)
                Timer = new TimerData();

            Timer
                .SetTick(PlayerStatus.Inst.gameObject)
                .SetAction(onEnd: () => PlayerStatus.RemoveEffect(this))
                .EndTime = endTime;
        }
    }

    public abstract void OnStart();
    public virtual void OnEnd()
    {
        Timer?.SetActive(false);
    }
}

