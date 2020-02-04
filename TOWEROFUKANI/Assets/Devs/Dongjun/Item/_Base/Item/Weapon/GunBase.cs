using UnityEngine;

public abstract class GunController<T> : WeaponController<T> where T : GunItem { }
public abstract class GunItem : WeaponItem
{
    #region Var: Animation Names
    public readonly string ANIM_Idle = "Idle";
    public readonly string ANIM_Shoot = "Shoot";
    public readonly string ANIM_Reload = "Reload";
    public readonly string ANIM_SwapMagazine = "SwapMagazine";
    #endregion

    #region Var: Stats
    // Timer
    public readonly TimerStat shootTimer = new TimerStat();
    public readonly TimerStat reloadTimer = new TimerStat();
    public readonly TimerStat swapMagazineTimer = new TimerStat();

    // Bullet Data
    public ProjectileData bulletData;

    [Header("Ammo Data")]
    public int loadedBullets;
    public bool isBulletLoaded;
    public IntStat magazineSize;
    #endregion

    #region Method: Unity
    protected virtual void Start()
    {
        // Init Timer
        shootTimer.SetTick(gameObject).ToEnd();
        reloadTimer.SetTick(gameObject);
        swapMagazineTimer.SetTick(gameObject);

        // Init Ammo
        loadedBullets = magazineSize.Value;
    }
    #endregion

    #region Method: Stats
    public override void ResetStats()
    {
        attackData.Reset();
        shootTimer.EndTime.Reset();
        reloadTimer.EndTime.Reset();
        swapMagazineTimer.EndTime.Reset();
        bulletData.Reset();
        magazineSize.Reset();
    }
    #endregion
}
