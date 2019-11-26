using UnityEngine;

public class WeaponProjectile : Projectile
{
    protected override bool CheckHit(RaycastHit2D hit)
    {
        IDamage damageMob = hit.collider.GetComponent<IDamage>();
        if (damageMob == null)
            return false;

        // Set Stat
        PlayerStats.DamageDealt = attackData.damage.Value;

        // Trigger Item Effect
        ItemEffectManager.Trigger(PlayerActions.Hit);

        // Damage Mob
        damageMob.Hit(PlayerStats.DamageDealt);
        return true;
    }
}
