using UnityEngine;

[System.Serializable]
public class GunData : ItemData
{

}
public abstract class GunItem : WeaponItem
{
    public TimerStat shootTimer;
    public TimerStat reloadTimer;
    public TimerStat swapMagazineTimer;

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

public abstract class GunController : WeaponObject<GunItem>
{

}
