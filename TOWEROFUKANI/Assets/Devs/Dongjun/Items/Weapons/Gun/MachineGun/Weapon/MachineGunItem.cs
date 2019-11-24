using UnityEngine;

public class MachineGunItem : GunItem
{
    #region Var: Inspector
    [Header("Ammo Belt")]
    public Transform ammoBelt;
    public float ammoBeltAmmoCount;
    #endregion

    #region Var: Properties
    public float AmmoBeltMaxY => 0.0625f * ammoBeltAmmoCount;
    #endregion

    public override void InitStats()
    {
        // Timmer Data
        shootTimer.EndTime = new FloatStat(0.15f, min: 0.01f);
        reloadTimer.EndTime = new FloatStat(0.5f, min: 0.01f);
        swapMagazineTimer.EndTime = new FloatStat(1f, min: 0.01f);

        // Bullet Data
        bulletData = new WeaponProjectileData()
        {
            attackData = new AttackData(1),
            moveSpeed = new FloatStat(45f, min: 0f),
            maxTravelDist = new FloatStat(10f, min: 0f),
        };

        // Ammo Data
        magazineSize = new IntStat(20, min: 0);

        // Upgrade Per Count
        switch (Count)
        {
            case 1:
                break;
            case 2:
                shootTimer.EndTime = new FloatStat(0.1f, min: 0.01f);
                break;
            default:
                shootTimer.EndTime = new FloatStat(0.05f, min: 0.01f);
                break;
        }
    }
}
