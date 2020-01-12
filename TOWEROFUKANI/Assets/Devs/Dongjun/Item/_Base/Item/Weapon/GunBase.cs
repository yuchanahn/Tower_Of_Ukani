using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunController<TGun> : WeaponController<TGun> where TGun : GunItem { }
public abstract class GunItem : WeaponItem
{
    #region Var: Stats
    // Timer
    public TimerStat shootTimer = new TimerStat();
    public TimerStat reloadTimer = new TimerStat();
    public TimerStat swapMagazineTimer = new TimerStat();

    // Bullet Data
    public ProjectileData bulletData;

    [Header("Ammo Data")]
    public int loadedBullets;
    public bool isBulletLoaded;
    public IntStat magazineSize;
    #endregion

    #region Var: Anim Clip Names
    public readonly string ANIM_Idle = "Idle";
    public readonly string ANIM_Shoot = "Shoot";
    public readonly string ANIM_Reload = "Reload";
    public readonly string ANIM_SwapMagazine = "SwapMagazine";
    #endregion

    #region Method: Unity
    protected override void Start()
    {
        base.Start();

        #region Init: Stats
        // Init Timer
        shootTimer.SetTick(gameObject);
        shootTimer.ToEnd();
        reloadTimer.SetTick(gameObject);
        swapMagazineTimer.SetTick(gameObject);

        // Init Ammo
        loadedBullets = magazineSize.Value;
        #endregion
    }
    #endregion
}
