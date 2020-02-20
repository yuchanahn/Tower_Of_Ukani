using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus_Knockback : PlayerStatusEffect
{
    KnockbackMode mode;
    Vector2 knockbackDir;
    AnimationCurve speedCurve;

    public PlayerStatus_Knockback(
        StatusID id,
        GameObject caster,
        KnockbackMode mode,
        Vector2 knockbackDir,
        AnimationCurve speedCurve,
        float endTime) : base(id, caster, StatusType.Debuff, endTime)
    {
        this.mode = mode;
        this.knockbackDir = knockbackDir;
        this.speedCurve = speedCurve;
    }

    public override void OnStart()
    {
        PlayerStatus.Inst.Konckback_Add(this, mode, knockbackDir, speedCurve);
    }
    public override void OnEnd()
    {
        base.OnEnd();
        PlayerStatus.Inst.Konckback_Remove(this);
    }
}
