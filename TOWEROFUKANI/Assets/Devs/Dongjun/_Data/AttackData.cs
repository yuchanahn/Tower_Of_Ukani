using System;
using System.Collections.Generic;
using UnityEngine;

public struct AttackData
{
    // 데미지
    public FloatStat damage;

    // Damage Type
    // Absorb Corpse
    // Knockback
    // Penetration
    // Etc...

    public AttackData(float damage = 0)
    {
        this.damage = new FloatStat(damage, min: 0);
    }

    public void Reset()
    {
        damage.Reset();
    }
}
