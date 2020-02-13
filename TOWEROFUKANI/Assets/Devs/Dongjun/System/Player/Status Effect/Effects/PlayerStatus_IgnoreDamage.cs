using System;
using UnityEngine;

public class PlayerStatus_IgnoreDamage : PlayerStatusEffect
{
    public PlayerStatus_IgnoreDamage(
        StatusID id,
        GameObject caster,
        float endTime = 0) : base(id, caster, StatusType.Buff, endTime, null, null)
    { }

    public override void OnStart()
    {
        base.OnStart();
        PlayerStatus.Inst.IgnoreDamage_Add();
    }
    public override void OnEnd()
    {
        base.OnEnd();
        PlayerStatus.Inst.IgnoreDamage_Remove();
    }
}
