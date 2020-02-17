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
    // Bullet Data
    public ProjectileData BulletData
    { get; protected set; }

    // Timer
    public readonly TimerStat Timer_Shoot = new TimerStat();
    public readonly TimerStat Timer_Reload = new TimerStat();
    public readonly TimerStat Timer_SwapMagazine = new TimerStat();

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
        Timer_Shoot.SetTick(gameObject).ToEnd();
        Timer_Reload.SetTick(gameObject).SetActive(false);
        Timer_SwapMagazine.SetTick(gameObject).SetActive(false);

        // Init Ammo
        LoadedBullets = MagazineSize.Value;
    }

    public override void ResetStats()
    {
        base.ResetStats();

        BulletData.Reset();

        Timer_Shoot.EndTime.Reset();
        Timer_Reload.EndTime.Reset();
        Timer_SwapMagazine.EndTime.Reset();

        MagazineSize.Reset();
    }

    public virtual void ReloadFull()
    {
        LoadedBullets = MagazineSize.Value;
    }
    public virtual void Reload(int amount)
    {
        LoadedBullets = amount;
        LoadedBullets = Mathf.Min(MagazineSize.Value);
    }
    public virtual void UseBullet(int amount)
    {
        amount = Mathf.Abs(amount);

        if (LoadedBullets > 0)
            LoadedBullets -= amount;
    }
}
