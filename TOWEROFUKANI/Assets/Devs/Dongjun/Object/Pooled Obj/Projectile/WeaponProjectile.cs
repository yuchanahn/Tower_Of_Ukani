using UnityEngine;

public class WeaponProjectile : Projectile
{
    protected override bool CheckCreatureHit(RaycastHit2D hit)
    {
        return PlayerStats.Inst.DealDamage(attackData, hit.collider.gameObject,
            PlayerActions.WeaponHit);
    }
}
