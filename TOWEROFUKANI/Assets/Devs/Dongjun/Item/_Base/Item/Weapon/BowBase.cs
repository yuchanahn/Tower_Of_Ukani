using UnityEngine;

public abstract class BowController<T> : WeaponController<T>
    where T : BowItem
{ }

public abstract class BowItem : WeaponItem
{
    #region Var: Animation Names
    public readonly string ANIM_Idle = "Idle";
    public readonly string ANIM_Shoot = "Shoot";
    public readonly string ANIM_Draw = "Pull";
    #endregion

    #region Var: Inspector
    [Header("Visual")]
    [SerializeField] protected GameObject arrowVisual;
    #endregion

    #region Var: Stats
    // Timer
    public readonly TimerStat shootTimer = new TimerStat();
    public readonly TimerStat drawTimer = new TimerStat();

    // Arrow Data
    public ProjectileData arrowData;
    #endregion

    #region Var: Bow Data
    [HideInInspector] public float drawPower = 0;
    #endregion

    #region Prop: 
    public GameObject ArrowVisual => arrowVisual;
    #endregion

    #region Method: Unity
    protected virtual void Start()
    {
        // Init Timer
        shootTimer.SetTick(gameObject).ToEnd();
        drawTimer.SetTick(gameObject);
    }
    #endregion

    #region Method: Stats
    public override void ResetStats()
    {
        attackData.Reset();
        shootTimer.EndTime.Reset();
        drawTimer.EndTime.Reset();
        arrowData.Reset();
    }
    #endregion
}
