using UnityEngine;

public class WeaponProjectile : Projectile
{
    protected override bool CheckHit(RaycastHit2D hit)
    {
        if (hit.collider.GetComponent<StatusEffect_IgnoreHit>())
            return false;

        return PlayerStats.Inst.DealDamage(hit.collider.GetComponent<IDamage>(), attackData,
            PlayerActions.WeaponHit);
    }
}
