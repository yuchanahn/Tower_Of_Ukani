using UnityEngine;

public class WeaponProjectile : Projectile
{
    protected override bool CheckHit(RaycastHit2D hit)
    {
        return PlayerStats.DealDamage(hit.collider.GetComponent<IDamage>(), attackData,
            PlayerActions.WeaponHit);
    }
}
