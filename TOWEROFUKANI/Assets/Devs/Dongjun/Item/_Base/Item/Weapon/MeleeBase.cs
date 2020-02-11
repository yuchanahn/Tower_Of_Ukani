using UnityEngine;

public abstract class MeleeController<T> : WeaponController<T> where T : MeleeItem { }
public abstract class MeleeItem : WeaponItem
{
    #region Var: Animation Names
    public readonly string ANIM_Neutral = "Neutral";
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
