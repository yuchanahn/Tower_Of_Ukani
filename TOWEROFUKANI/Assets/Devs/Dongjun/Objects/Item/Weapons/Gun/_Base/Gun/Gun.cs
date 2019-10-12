using UnityEngine;
using NaughtyAttributes;

public abstract class Gun : Weapon
{
    [BoxGroup("Timer")] public TimerStat shootTimer;
    [BoxGroup("Timer")] public TimerStat reloadTimer;
    [BoxGroup("Timer")] public TimerStat swapMagazineTimer;

    [BoxGroup("Ammo")] public int magazineSize;
    [BoxGroup("Ammo")] public int loadedBullets;
    [BoxGroup("Ammo")] public bool isBulletLoaded;

    protected override void Start()
    {
        base.Start();

        // Init Stat
        shootTimer.EndTime.Init();
        reloadTimer.EndTime.Init();
        swapMagazineTimer.EndTime.Init();

        // Init Timer
        shootTimer.Init(gameObject);
        reloadTimer.Init(gameObject);
        swapMagazineTimer.Init(gameObject);

        // Init Ammo
        loadedBullets = magazineSize;
    }
}
