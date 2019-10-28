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

        shootTimer.StartAsEnded = true;
        shootTimer.EndTime = new FloatStat(0.05f, min: 0.01f);
        reloadTimer.EndTime = new FloatStat(0.5f, min: 0.01f);
        swapMagazineTimer.EndTime = new FloatStat(1f, min: 0.01f);

        magazineSize = new IntStat(20, min: 0);
    }
}
