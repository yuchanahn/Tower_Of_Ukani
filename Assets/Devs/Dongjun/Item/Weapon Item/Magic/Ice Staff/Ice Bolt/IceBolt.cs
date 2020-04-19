using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBolt : WeaponProjectile
{
    protected override bool DamageCreature(GameObject hit)
    {
        bool hasHit = base.DamageCreature(hit);

        if (hasHit)
        {

        }

        return hasHit;
    }
}
