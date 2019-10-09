using UnityEngine;
using NaughtyAttributes;

public abstract class Gun : Weapon
{
    [BoxGroup("Timer")] public TimerData shootTimer;
    [BoxGroup("Timer")] public TimerData reloadTimer;
    [BoxGroup("Timer")] public TimerData swapMagazineTimer;

    [BoxGroup("Ammo")] public int magazineSize;
    [BoxGroup("Ammo")] public int loadedBullets;
    [BoxGroup("Ammo")] public bool isBulletLoaded;

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
