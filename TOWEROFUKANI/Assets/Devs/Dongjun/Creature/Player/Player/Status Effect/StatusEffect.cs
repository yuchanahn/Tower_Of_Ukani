using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType
{
    Buff,
    Debuff
}

public class StatusID { }

public class StatusEffect
{
    public StatusID ID
    { get; protected set; }
    public MobAction MobAction
    { get; protected set; }
    public GameObject Caster
    { get; protected set; }
    public StatusType StatusType
    { get; protected set; }

    public Type AfterThis
    { get; protected set; } = null;

    public Action OnStart
    { get; protected set; }
    public Action OnEnd
    { get; protected set; }
    public Action OnAction
    { get; protected set; }

    public TimerData Timer
    { get; protected set; } = null;
    public bool EndOnCasterDeath
    { get; protected set; }

    public StatusEffect(
        StatusID id,
        MobAction mobAction,
        GameObject caster,
        StatusType statusType,
        float endTime = 0,
        Action onStart = null,
        Action onEnd = null,
        Action onAction = null,
        Type afterThis = null)
    {
        ID = id;
        MobAction = mobAction;
        Caster = caster;
        StatusType = statusType;
        OnStart = onStart;
        OnEnd = onEnd;
        OnAction = onAction;
        AfterThis = afterThis;

        if (endTime != 0)
        {
            Timer = new TimerData();
            Timer.EndTime = endTime;
            Timer.SetTick(PlayerStatus.Inst.gameObject, TickType.Update).
                SetAction(OnEnd: () => 
                {
                    OnEnd?.Invoke();
                    PlayerStatus.Inst.RemoveFromList(id, mobAction, this);
                });
        }
    }
}

