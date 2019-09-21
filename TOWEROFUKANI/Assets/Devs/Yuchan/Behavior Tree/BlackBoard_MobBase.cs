using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard_MobBase : BlackBoard_Base
{
    Mob_Base mob;

    private void Awake() 
    {
        mob = GetComponent<Mob_Base>();
    }

    internal void SV_AgroCheck()
    {
    }
    internal bool CN_InHurt()
    {
        return false;
    }
    internal bool CN_InFollowRange()
    {
        return false;
    }
    internal bool CN_RandomMove()
    {
        mob.RandomMove();
        return true;
    }
    internal bool TA_Attack()
    {
        return false;
    }
    internal bool TA_Hurt()
    {
        return false;
    }
    internal bool CN_InAttackRange()
    {
        return false;
    }

    internal bool TA_Run()
    {
        return false;
    }

    internal bool TA_Follow()
    {
        return false;
    }
    #region Action

    #endregion

    #region Condition

    #endregion

    #region Service

    #endregion
}