using System;
using UnityEngine;

public class PlayerStatus_AbsorbDamage : PlayerStatusEffect
{
    public PlayerStatus_AbsorbDamage(
        StatusID id,
        GameObject caster,
        float endTime = 0) : base(id, caster, StatusType.Buff, endTime, null, null)
    { }

    public override void OnStart()
    {
        base.OnStart();
        PlayerStatus.Inst.AbsorbDamage_Add();
    }
    public override void OnEnd()
    {
        base.OnEnd();
        PlayerStatus.Inst.AbsorbDamage_Remove();
    }
}
