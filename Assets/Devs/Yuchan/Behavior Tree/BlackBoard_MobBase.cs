﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard_MobBase : BlackBoard_Base
{
    protected GroundMob_Base mob;

    private void Awake()
    {
        mob = GetComponent<GroundMob_Base>();
    }

    #region Action

    public bool TA_RandMove()   => mob.MoveRandom();
    public bool TA_Attack()     => mob.Attack();
    public bool TA_Idle()       => mob.IdleRandom();
    public bool TA_Falling() => mob.Falling();
    public bool TA_Follow() => mob.Follow();
    public bool TA_Hurt()       => true;
    public bool TA_SENoAct() => mob.SENoAct();

    #endregion

    #region Condition

    public bool CN_IsFollow() => mob.CanFollow;
    public bool CN_IsAttack() => mob.CanAttack;
    public bool CN_IsHurted() => mob.Hurting();
    public bool CN_IsNotAttacking() => !mob.IsAttacking;

    #endregion

    #region Service

    #endregion
}