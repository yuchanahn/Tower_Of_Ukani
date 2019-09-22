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
    internal bool TA_Follow() => mob.FollowPlayer();
    internal bool TA_Attack() => mob.Attack();
    internal bool TA_Hurt() => true;
    #endregion

    #region Condition
    internal bool CN_InHurt()
    {
        return false;
    }
    internal bool CN_InFollowRange() => mob.InFollowRange;

    internal bool TA_RandomMove()
    {
        return true;
    }
    virtual internal bool CN_InAttackAble() => mob.IsKeepAttack ? true : mob.StartAttacking = mob.InAttackRange;
    #endregion

    #region Service
    internal void SV_AgroCheck()
    {
    }
    internal void SV_SetRandomDir()
    {
        mob.CurDir = mob.RandomDir;
    }
    #endregion
}