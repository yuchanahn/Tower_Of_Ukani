using System;
using UnityEngine;

public class StatusStun : StatusEffect
{
    public StatusStun(
        StatusID id,
        MobAction mobAction,
        GameObject caster,
        StatusType statusType,
        float endTime = 0,
        Action onStart = null,
        Action onEnd = null,
        Action onAction = null,
        Type afterThis = null) : base(id, mobAction, caster, statusType, endTime, onStart, onEnd, onAction, afterThis)
    {
        PlayerStatus.Inst.AddStun(caster);

        if (Timer == null)
            Timer = new TimerData();

        Timer.EndTime = endTime;
        Timer.SetAction(OnEnd: () =>
        {
            OnEnd?.Invoke();
            PlayerStatus.Inst.RemoveStun(caster);
            PlayerStatus.Inst.RemoveFromList(id, mobAction, this);
        });
    }
}
