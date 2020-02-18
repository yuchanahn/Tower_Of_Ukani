using UnityEngine;

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
    // Arrow Data
    public ProjectileData arrowData;

    // Timer
    public readonly TimerStat Timer_Shoot = new TimerStat();
    public readonly TimerStat Timer_Draw = new TimerStat();
    #endregion

    #region Prop: 
    public float DrawPower { get; private set; } = 0;

    public GameObject ArrowVisual => arrowVisual;
    #endregion

    protected virtual void Start()
    {
        // Init Timer
        Timer_Shoot.SetTick(gameObject).ToEnd();
        Timer_Draw
            .SetTick(gameObject)
            .SetAction(
                onStart: () => DrawPower = 0,
                onTick: () => DrawPower = Timer_Draw.CurTime / Timer_Draw.EndTime.Value)
            .SetActive(false);
    }

    public override void ResetStats()
    {
        base.ResetStats();

        arrowData.Reset();
        Timer_Shoot.EndTime.Reset();
        Timer_Draw.EndTime.Reset();
    }
}
