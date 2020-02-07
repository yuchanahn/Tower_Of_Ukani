using System;
using UnityEngine;

public class StatusStun : StatusEffect
{
    public StatusStun(
        StatusID id,
        MobAction mobAction,
        GameObject caster,
        StatusType statusType,
        Action onStart = null,
        Action onEnd = null,
        Action onAction = null,
        Type afterThis = null,
    float endTime = 0) : base(id, mobAction, caster, statusType, onStart, onEnd, onAction, afterThis, endTime)
    {
        PlayerStatus.Inst.AddStunCount();

        Timer?.SetAction(OnEnd: () =>
        {
            OnEnd?.Invoke();
            PlayerStatus.Inst.RemoveStunCount();
            PlayerStatus.Inst.RemoveFromList(id, mobAction, this);
        });
    }
}
