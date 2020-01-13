using System;
using UnityEngine;

public struct AttackData
{
    public FloatStat damage;
    public bool absorbCorpses;
    public Action<Corpse> onAbsorb; // Corpse 매개변수에 인스턴스가 아니라 프리팹을 넣어야 함!!!
    // Damage Type
    // Absorb Corpse
    // Knockback
    // Penetration
    // Etc...

    public AttackData(float damage = 0)
    {
        this.damage = new FloatStat(damage, min: 0);

        absorbCorpses = false;
        onAbsorb = null;
    }
}
