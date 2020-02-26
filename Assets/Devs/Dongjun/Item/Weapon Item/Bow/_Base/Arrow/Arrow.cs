using UnityEngine;

public class Arrow : WeaponProjectile
{
    protected override bool DamageCreature(GameObject hit)
    {
        return PlayerStats.Inst.DealDamage(attackData, hit,
            PlayerActions.WeaponHit,
            PlayerActions.BowHit);
    }
}
