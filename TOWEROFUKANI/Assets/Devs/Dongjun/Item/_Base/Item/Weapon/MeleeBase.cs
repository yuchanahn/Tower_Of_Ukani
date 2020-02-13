using UnityEngine;

public abstract class MeleeController<T> : WeaponController<T> where T : MeleeItem { }
public abstract class MeleeItem : WeaponItem
{
    #region Var: Animation Names
    public readonly string ANIM_Neutral = "Neutral";
    #endregion

    #region Var: Stats
    // Attack Data
    public AttackData attackData_Heavy;
    public AttackData attackData_Slam;
    public AttackData attackData_Dash;

    // Timer
    public readonly TimerStat basicAttackTimer = new TimerStat();
    public readonly TimerStat heavyAttackTimer = new TimerStat();
    #endregion

    #region Method: Unity
    protected virtual void Start()
    {
        // Init Timer
        basicAttackTimer.SetTick(gameObject).ToEnd();
        heavyAttackTimer.SetTick(gameObject).ToEnd();
    }
    #endregion

    #region Method: Stats
    public override void ResetStats()
    {
        attackData.Reset();
        attackData_Heavy.Reset();
        attackData_Slam.Reset();
        attackData_Dash.Reset();

        basicAttackTimer.EndTime.Reset();
        heavyAttackTimer.EndTime.Reset();
    }
    #endregion
}
