using UnityEngine;

public abstract class GunItem : WeaponItem
{
    #region Var: Animation
    public readonly string ANIM_Idle = "Idle";
    public readonly string ANIM_Shoot = "Shoot";
    public readonly string ANIM_Reload = "Reload";
    public readonly string ANIM_SwapMagazine = "SwapMagazine";
    #endregion

    #region Var: Stats
    // Timer
    public readonly TimerStat Main_Shoot_CD = new TimerStat();
    public readonly TimerStat Main_Reload_Dur = new TimerStat();
    public readonly TimerStat Main_SwapMagazine_Dur = new TimerStat();

    // Bullet Data
    public ProjectileData Main_BulletData
    { get; protected set; }

    // Ammo Data
    public IntStat Main_MagazineSize
    { get; protected set; }
    public int LoadedBullets
    { get; protected set; }
    public bool IsBulletLoaded => LoadedBullets != 0;
    #endregion

    protected virtual void Start()
    {
        // Init Timer
        Main_Shoot_CD.SetTick(gameObject).ToEnd();
        Main_Reload_Dur.SetTick(gameObject).SetActive(false);
        Main_SwapMagazine_Dur.SetTick(gameObject).SetActive(false);

        // Init Ammo
        LoadedBullets = Main_MagazineSize.Value;
    }

    public override void ResetStats()
    {
        base.ResetStats();
        Main_Shoot_CD.EndTime.Reset();
        Main_Reload_Dur.EndTime.Reset();
        Main_SwapMagazine_Dur.EndTime.Reset();

        Main_BulletData.Reset();
        Main_MagazineSize.Reset();
    }

    public virtual void ReloadFull()
    {
        LoadedBullets = Main_MagazineSize.Value;
    }
    public virtual void Reload(int amount)
    {
        LoadedBullets = amount;
        LoadedBullets = Mathf.Min(LoadedBullets, Main_MagazineSize.Value);
    }
    public virtual void UseBullet(int amount)
    {
        amount = Mathf.Abs(amount);

        if (LoadedBullets > 0)
            LoadedBullets -= amount;
    }
}
