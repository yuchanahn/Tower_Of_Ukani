using UnityEngine;

public abstract class GunItem : WeaponItem
{
    // Timer Data
    public TimerStat shootTimer;
    public TimerStat reloadTimer;
    public TimerStat swapMagazineTimer;

    // Bullet Data
    public WeaponProjectileData bulletData;

    // Ammo Data
    public IntStat magazineSize;
    public int loadedBullets;
    public bool isBulletLoaded;

    protected void Start()
    {
        // Init Timer
        shootTimer.Init(gameObject);
        reloadTimer.Init(gameObject);
        swapMagazineTimer.Init(gameObject);

        // Init Ammo
        loadedBullets = magazineSize.Value;
    }
}

public abstract class GunController : WeaponController<GunItem>
{

}
