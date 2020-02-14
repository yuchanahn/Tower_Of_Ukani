using UnityEngine;

public class Bullet : WeaponProjectile
{
    protected override bool CheckCreatureHit(RaycastHit2D hit)
    {
        return PlayerStats.Inst.DealDamage(attackData, hit.collider.gameObject,
            PlayerActions.WeaponHit,
            PlayerActions.GunHit);
    }
}
