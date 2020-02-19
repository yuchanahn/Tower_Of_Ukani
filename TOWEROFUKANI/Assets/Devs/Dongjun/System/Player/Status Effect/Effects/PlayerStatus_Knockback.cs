using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus_Knockback : PlayerStatusEffect
{
    int power;
    Vector2 knockbackVector;

    public PlayerStatus_Knockback(
        StatusID id,
        GameObject caster,
        int power,
        Vector2 knockbackVector,
        float endTime = 0) : base(id, caster, StatusType.Debuff, endTime)
    {
        this.power = power;
        this.knockbackVector = knockbackVector;
    }

    public override void OnStart()
    {
        PlayerStatus.Inst.Konckback_Add(power, knockbackVector);
    }
    public override void OnEnd()
    {
        PlayerStatus.Inst.Konckback_Remove(power);
    }
}
