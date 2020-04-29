using System;
using System.Collections.Generic;
using UnityEngine;

public struct AttackData
{
    // 데미지
    public FloatStat damage;
    public GameObject damageDealer;
    public bool playHitAnim;

    // Damage Type
    // Absorb Corpse
    // Penetration
    // Etc...

    public AttackData(float damage = 0, GameObject damageDealer = null, bool playHitAnim = false)
    {
        this.damage = new FloatStat(damage, min: 0);
        this.damageDealer = damageDealer;
        this.playHitAnim = playHitAnim;
    }

    public void Reset()
    {
        damage.Reset();
    }
}
