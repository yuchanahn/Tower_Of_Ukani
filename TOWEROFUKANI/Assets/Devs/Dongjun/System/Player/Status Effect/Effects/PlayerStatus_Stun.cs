using System;
using UnityEngine;

public class PlayerStatus_Stun : PlayerStatusEffect
{
    public PlayerStatus_Stun(
        StatusID id,
        GameObject caster,
        float endTime = 0) : base(id, caster,  StatusType.Debuff, endTime)
    { }

    public override void OnStart()
    {
        PlayerStatus.Inst.Stun_Add();
    }
    public override void OnEnd()
    {
        PlayerStatus.Inst.Stun_Remove();
    }
}
