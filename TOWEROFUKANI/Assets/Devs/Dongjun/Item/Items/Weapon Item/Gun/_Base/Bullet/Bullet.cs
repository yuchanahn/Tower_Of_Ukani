using UnityEngine;

public class Bullet : WeaponProjectile
{
    protected override bool CheckHit(RaycastHit2D hit)
    {
        return PlayerStats.Inst.DealDamage(hit.collider.GetComponent<IDamage>(), attackData,
            PlayerActions.WeaponHit,
            PlayerActions.GunHit);
    }
}
