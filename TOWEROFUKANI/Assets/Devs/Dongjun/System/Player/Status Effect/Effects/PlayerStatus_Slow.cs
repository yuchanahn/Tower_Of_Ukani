using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus_Slow : PlayerStatusEffect
{
    public float SlowAmount
    { get; private set; } = 0;

    public PlayerStatus_Slow(
        StatusID id,
        GameObject caster,
        float slowAmount,
        float endTime = 0) : base(id, caster, StatusType.Debuff, endTime, null, null)
    {
        this.SlowAmount = slowAmount;
    }

    public override void OnStart()
    {
        base.OnStart();
        PlayerStatus.Inst.Slow_Add(this);
    }
    public override void OnEnd()
    {
        base.OnEnd();
        PlayerStatus.Inst.Slow_Remove(this);
    }
}
