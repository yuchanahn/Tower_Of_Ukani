using System;
using System.Collections.Generic;
using UnityEngine;

public struct AttackData
{
    // 데미지
    public FloatStat damage;
    public Type damageDealer;

    // Damage Type
    // Absorb Corpse
    // Penetration
    // Etc...

    public AttackData(float damage = 0, Type damageDealer = null)
    {
        this.damage = new FloatStat(damage, min: 0);
        this.damageDealer = damageDealer;
    }

    public void Reset()
    {
        damage.Reset();
    }
}
