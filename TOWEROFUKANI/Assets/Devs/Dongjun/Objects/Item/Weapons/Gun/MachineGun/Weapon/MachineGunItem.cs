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

    protected override void Awake()
    {
        base.Awake();

        // Timmer Data
        shootTimer.StartAsEnded = true;
        shootTimer.EndTime = new FloatStat(0.05f, min: 0.01f);
        reloadTimer.EndTime = new FloatStat(0.5f, min: 0.01f);
        swapMagazineTimer.EndTime = new FloatStat(1f, min: 0.01f);

        // Bullet Data
        bulletData.damage = new IntStat(1, min: 0);
        bulletData.moveSpeed = new FloatStat(45f, min: 0f);
        bulletData.maxTravelDist = new FloatStat(10f, min: 0f);

        // Ammo Data
        magazineSize = new IntStat(20, min: 0);
    }
}
