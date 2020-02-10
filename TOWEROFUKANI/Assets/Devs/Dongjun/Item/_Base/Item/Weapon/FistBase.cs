using UnityEngine;

public abstract class FistController<T> : WeaponController<T> where T : FistItem { }
public abstract class FistItem : WeaponItem
{
    #region Var: Animation Names
    public readonly string ANIM_Neutral = "Idle";
    #endregion

    #region Var: Stats
    // Timer
    public readonly TimerStat basicAttackTimer = new TimerStat();
    #endregion

    #region Method: Unity
    protected virtual void Start()
    {
        // Init Timer
        basicAttackTimer.SetTick(gameObject).ToEnd();
    }
    #endregion

    #region Method: Stats
    public override void ResetStats()
    {
        attackData.Reset();
        basicAttackTimer.EndTime.Reset();
    }
    #endregion
}
