using System;
using UnityEngine;

public class PlayerStatus_AbsorbDamage : PlayerStatusEffect
{
    public PlayerStatus_AbsorbDamage(
        StatusID id,
        GameObject caster,
        float endTime = 0) : base(id, caster, StatusType.Buff, endTime)
    { }

    public override void OnStart()
    {
        PlayerStatus.Inst.AbsorbDamage_Add();
    }
    public override void OnEnd()
    {
        base.OnEnd();
        PlayerStatus.Inst.AbsorbDamage_Remove();
    }
}
