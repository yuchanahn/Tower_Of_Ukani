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
    { get; private set; }
    public MobAction MobAction
    { get; private set; }
    public GameObject Caster
    { get; protected set; }
    public StatusType StatusType
    { get; private set; }

    public Type AfterThis
    { get; private set; } = null;

    public Action OnStart
    { get; private set; }
    public Action OnEnd
    { get; private set; }
    public Action OnAction
    { get; private set; }

    public TimerData Timer
    { get; private set; } = null;
    public bool EndOnCasterDeath
    { get; private set; }

    public StatusEffect(
        StatusID id,
        MobAction mobAction,
        GameObject caster,
        StatusType statusType,
        Action onStart = null,
        Action onEnd = null,
        Action onAction = null,
        Type afterThis = null,
        int endTime = 0)
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
            Timer.SetTick(PlayerStatus.Inst.gameObject, TickType.Update).
                SetAction(OnEnd: () => 
                {
                    OnEnd?.Invoke();
                    PlayerStatus.Inst.RemoveFromList(id, mobAction, this);
                });
        }
    }
}
