using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Slime : Mob_Base
{
    private bool mbAttacking = false;
    private bool mbKeepAttackRun = true;
    private bool mbOnAttackRun = false;
    private int mDir = 0;

    public bool IsAttacking => mbAttacking;

    public bool AttackRun()
    {
        if(mbOnAttackRun)
        {
            mbOnAttackRun = false;
            CurDir = mDir;
            Jump();
            mAddSpeed = 2f;
        }
        return IsAttacking;
    }
    override protected void OnAttack()
    {
        base.OnAttack();
        mbAttacking = true;
        mbOnAttackRun = true;
        mDir = GM.PlayerPos.x > transform.position.x ? -1 : 1;
    }

    override public void OnGroundEnter()
    {
        base.OnGroundEnter();
        mbAttacking = false;
        mAddSpeed = 0;
    }
}