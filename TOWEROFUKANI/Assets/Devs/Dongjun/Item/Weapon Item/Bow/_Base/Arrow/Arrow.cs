using UnityEngine;

public class Arrow : WeaponProjectile
{
    protected override bool CheckCreatureHit(RaycastHit2D hit)
    {
        return PlayerStats.Inst.DealDamage(attackData, hit.collider.gameObject,
            PlayerActions.WeaponHit,
            PlayerActions.BowHit);
    }
}
