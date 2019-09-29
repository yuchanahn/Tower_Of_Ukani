using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard_MobBase : BlackBoard_Base
{
    protected Mob_Base mob;

    private void Awake()
    {
        mob = GetComponent<Mob_Base>();
    }

    #region Action

    public bool TA_RandMove()   => mob.MoveRandom();
    public bool TA_Attack()     => false;
    public bool TA_Idle()       => mob.IdleRandom();
    public bool TA_Follow()     => false;
    public bool TA_Hurt()       => false;

    #endregion

    #region Condition

    public bool CN_IsFollow() => false;
    public bool CN_IsAttack() => false;
    public bool CN_IsHurted() => false;

    #endregion

    #region Service

    #endregion
}