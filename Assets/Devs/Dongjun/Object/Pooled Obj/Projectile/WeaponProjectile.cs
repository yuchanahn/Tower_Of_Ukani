using UnityEngine;

public class WeaponProjectile : Projectile
{
    protected override bool DamageCreature(GameObject hit)
    {
        return PlayerStats.Inst.DealDamage(attackData, hit,
            PlayerActions.WeaponHit);
    }
}
