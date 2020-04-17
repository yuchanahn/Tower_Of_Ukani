using UnityEngine;

public abstract class MeleeItem : WeaponItem
{
    #region Var: Animation Names
    public readonly string ANIM_Neutral = "Neutral";
    #endregion

    #region Var: Stats
    // Timer
    public readonly TimerStat Basic_Dur = new TimerStat();
    public readonly TimerStat Heavy_Dur = new TimerStat();
    public readonly TimerStat Slam_Dur = new TimerStat();

    // Attack Data
    public AttackData Heavy_AttackData;
    public AttackData Slam_AttackData;
    public AttackData Dash_AttackData;

    // Slam
    public readonly float SlamDownVel = 30f;
    #endregion

    #region Method: Unity
    protected virtual void Start()
    {
        // Init Timer
        Basic_Dur.SetTick(gameObject).SetActive(false);
        Heavy_Dur.SetTick(gameObject).SetActive(false);
        Slam_Dur.SetTick(gameObject).SetActive(false);
    }
    #endregion

    #region Method: Stats
    public override void ResetStats()
    {
        base.ResetStats();
        Basic_Dur.EndTime.Reset();
        Heavy_Dur.EndTime.Reset();

        Heavy_AttackData.Reset();
        Slam_AttackData.Reset();
        Dash_AttackData.Reset();
    }
    #endregion
}
