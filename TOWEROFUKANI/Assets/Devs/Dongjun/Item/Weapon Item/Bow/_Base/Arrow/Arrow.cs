using UnityEngine;

public class Arrow : WeaponProjectile
{
    protected override bool CheckHit(RaycastHit2D hit)
    {
        return PlayerStats.Inst.DealDamage(attackData, hit.collider.gameObject,
            PlayerActions.WeaponHit,
            PlayerActions.BowHit);
    }
}
