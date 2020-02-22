using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus_Knockback : PlayerStatusEffect
{
    KnockbackMode mode;
    bool resetGravity;
    Vector2 knockbackDir;
    AnimationCurve speedCurve;

    public PlayerStatus_Knockback(
        StatusID id,
        GameObject caster,
        KnockbackMode mode,
        bool resetGravity,
        Vector2 knockbackDir,
        AnimationCurve speedCurve) : base(id, caster, StatusType.Debuff, speedCurve.keys[speedCurve.keys.Length - 1].time)
    {
        this.mode = mode;
        this.resetGravity = resetGravity;
        this.knockbackDir = knockbackDir;
        this.speedCurve = speedCurve;
    }

    public override void OnStart()
    {
        PlayerStatus.Inst.Konckback_Add(this, mode, resetGravity, knockbackDir, speedCurve);
    }
    public override void OnEnd()
    {
        base.OnEnd();
        PlayerStatus.Inst.Konckback_Remove();
    }
}
