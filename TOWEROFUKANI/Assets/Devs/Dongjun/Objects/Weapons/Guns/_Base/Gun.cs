using UnityEngine;

public abstract class Gun : Weapon
{
    [Header("Timer")]
    public TimerData shootTimer;
    public TimerData reloadTimer;
    public TimerData swapMagazineTimer;

    [Header("Ammo")]
    public int magazineSize;
    public int loadedBullets;
    public bool isBulletLoaded;

    protected override void Start()
    {
        base.Start();

        // Init Timer
        shootTimer.Init(gameObject);
        reloadTimer.Init(gameObject);
        swapMagazineTimer.Init(gameObject);

        // Init Ammo
        loadedBullets = magazineSize;
    }
}
