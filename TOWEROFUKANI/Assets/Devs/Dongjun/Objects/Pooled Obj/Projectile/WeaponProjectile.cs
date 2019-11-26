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
        damageMob.Hit(PlayerStats.DamageDealt); // 여기서 체력이나 불리언 반환 해줘!!

        // TO DO:
        // Kill Item Effect Trigger
        return true;
    }
}
