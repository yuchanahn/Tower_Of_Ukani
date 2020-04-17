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
    public readonly TimerStat Main_Shoot_CD = new TimerStat();
    public readonly TimerStat Main_Draw_Dur = new TimerStat();

    // Arrow Data
    public ProjectileData Main_ArrowData;
    #endregion

    #region Prop: 
    public float DrawPower { get; private set; } = 0;

    public GameObject ArrowVisual => arrowVisual;
    #endregion

    protected virtual void Start()
    {
        // Init Timer
        Main_Shoot_CD
            .SetTick(gameObject)
            .SetActive(false);
        Main_Draw_Dur
            .SetTick(gameObject)
            .SetAction(
                onStart: () => DrawPower = 0,
                onTick: () => DrawPower = Main_Draw_Dur.CurTime / Main_Draw_Dur.EndTime.Value)
            .SetActive(false);
    }

    public override void ResetStats()
    {
        base.ResetStats();

        Main_ArrowData.Reset();
        Main_Shoot_CD.EndTime.Reset();
        Main_Draw_Dur.EndTime.Reset();
    }
}
