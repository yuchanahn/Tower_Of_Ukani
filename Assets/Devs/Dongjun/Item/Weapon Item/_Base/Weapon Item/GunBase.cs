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
    public readonly TimerStat CD_Main_Shoot = new TimerStat();
    public readonly TimerStat Dur_Main_Reload = new TimerStat();
    public readonly TimerStat Dur_Main_SwapMagazine = new TimerStat();

    // Bullet Data
    public ProjectileData BulletData
    { get; protected set; }

    // Ammo Data
    public IntStat MagazineSize
    { get; protected set; }
    public int LoadedBullets
    { get; protected set; }
    public bool IsBulletLoaded => LoadedBullets != 0;
    #endregion

    protected virtual void Start()
    {
        // Init Timer
        CD_Main_Shoot.SetTick(gameObject).ToEnd();
        Dur_Main_Reload.SetTick(gameObject).SetActive(false);
        Dur_Main_SwapMagazine.SetTick(gameObject).SetActive(false);

        // Init Ammo
        LoadedBullets = MagazineSize.Value;
    }

    public override void ResetStats()
    {
        base.ResetStats();
        CD_Main_Shoot.EndTime.Reset();
        Dur_Main_Reload.EndTime.Reset();
        Dur_Main_SwapMagazine.EndTime.Reset();

        BulletData.Reset();
        MagazineSize.Reset();
    }

    public virtual void ReloadFull()
    {
        LoadedBullets = MagazineSize.Value;
    }
    public virtual void Reload(int amount)
    {
        LoadedBullets = amount;
        LoadedBullets = Mathf.Min(LoadedBullets, MagazineSize.Value);
    }
    public virtual void UseBullet(int amount)
    {
        amount = Mathf.Abs(amount);

        if (LoadedBullets > 0)
            LoadedBullets -= amount;
    }
}
