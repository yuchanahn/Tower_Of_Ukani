using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BowController<TBow> : WeaponController<TBow> where TBow : BowItem { }
public abstract class BowItem : WeaponItem
{
    #region Var: Inspector
    [Header("Visual")]
    [SerializeField] protected GameObject arrowVisual;
    #endregion

    #region Var: Stats
    // Timer
    public TimerStat shootTimer = new TimerStat();
    public TimerStat drawTimer = new TimerStat();

    // Arrow Data
    public ProjectileData arrowData;
    #endregion

    #region Var: Bow Data
    [HideInInspector] public float drawPower = 0;
    #endregion

    #region Var: Anim Clip Names
    public readonly string ANIM_Idle = "Idle";
    public readonly string ANIM_Shoot = "Shoot";
    public readonly string ANIM_Draw = "Pull";
    #endregion

    #region Var: Properties
    public GameObject ArrowVisual => arrowVisual;
    #endregion

    #region Method: Unity
    protected override void Start()
    {
        base.Start();

        #region Init: Stats
        // Init Timer
        shootTimer.SetTick(gameObject);
        shootTimer.ToEnd();
        drawTimer.SetTick(gameObject);
        #endregion
    }
    #endregion
}
