using UnityEngine;

public class Bullet : WeaponProjectile
{
    protected override bool DamageCreature(GameObject hit)
    {
        return PlayerStats.Inst.DealDamage(attackData, hit,
            PlayerActions.WeaponHit,
            PlayerActions.GunHit);
    }
}
