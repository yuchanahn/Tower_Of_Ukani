using UnityEngine;

public abstract class MeleeItem : WeaponItem
{
    #region Var: Animation Names
    public readonly string ANIM_Neutral = "Neutral";
    #endregion

    #region Var: Stats
    // Timer
    public readonly TimerStat Dur_Basic = new TimerStat();
    public readonly TimerStat Dur_Heavy = new TimerStat();
    public readonly TimerStat Dur_Slam = new TimerStat();

    // Attack Data
    public AttackData AttackData_Heavy;
    public AttackData AttackData_Slam;
    public AttackData AttackData_Dash;

    // Slam
    public readonly float SlamDownVel = 30f;
    #endregion

    #region Method: Unity
    protected virtual void Start()
    {
        // Init Timer
        Dur_Basic.SetTick(gameObject).SetActive(false);
        Dur_Heavy.SetTick(gameObject).SetActive(false);
        Dur_Slam.SetTick(gameObject).SetActive(false);
    }
    #endregion

    #region Method: Stats
    public override void ResetStats()
    {
        base.ResetStats();
        Dur_Basic.EndTime.Reset();
        Dur_Heavy.EndTime.Reset();

        AttackData_Heavy.Reset();
        AttackData_Slam.Reset();
        AttackData_Dash.Reset();
    }
    #endregion
}
