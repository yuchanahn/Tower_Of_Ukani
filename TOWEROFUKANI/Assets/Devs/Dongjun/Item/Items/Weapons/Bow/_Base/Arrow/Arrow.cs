using UnityEngine;

public class Arrow : WeaponProjectile
{
    protected override bool CheckHit(RaycastHit2D hit)
    {
        return PlayerStats.Inst.DealDamage(hit.collider.GetComponent<IDamage>(), attackData,
            PlayerActions.WeaponHit,
            PlayerActions.BowHit);
    }
}
