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
    // Timer
    public readonly TimerStat CD_Main_Shoot = new TimerStat();
    public readonly TimerStat Dur_Main_Draw = new TimerStat();

    // Arrow Data
    public ProjectileData arrowData;
    #endregion

    #region Prop: 
    public float DrawPower { get; private set; } = 0;

    public GameObject ArrowVisual => arrowVisual;
    #endregion

    protected virtual void Start()
    {
        // Init Timer
        CD_Main_Shoot
            .SetTick(gameObject)
            .SetActive(false);
        Dur_Main_Draw
            .SetTick(gameObject)
            .SetAction(
                onStart: () => DrawPower = 0,
                onTick: () => DrawPower = Dur_Main_Draw.CurTime / Dur_Main_Draw.EndTime.Value)
            .SetActive(false);
    }

    public override void ResetStats()
    {
        base.ResetStats();

        arrowData.Reset();
        CD_Main_Shoot.EndTime.Reset();
        Dur_Main_Draw.EndTime.Reset();
    }
}
