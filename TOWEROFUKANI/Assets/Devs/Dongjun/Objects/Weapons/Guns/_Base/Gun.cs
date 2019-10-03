public abstract class Gun : Weapon
{
    public GunData gunData;

    protected override void Start()
    {
        base.Start();

        // Init Timer
        gunData.shootTimer.Init(gameObject);
        gunData.reloadTimer.Init(gameObject);
        gunData.swapMagazineTimer.Init(gameObject);

        // Init Ammo
        gunData.loadedBullets = gunData.magazineSize;
    }
}
