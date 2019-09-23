using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Slime : Mob_Base
{
    [Header("Mob_Slime")]
    [SerializeField] float mAddSpeed_;

    private bool mbAttacking = false;
    private bool mbKeepAttackRun = true;
    private bool mbOnAttackRun = false;
    private int mDir = 0;

    public bool IsAttacking => mbAttacking;
    public override int RandomDir => IsAttacking ? mDir : base.RandomDir;

    public bool AttackRun()
    {
        if(mbOnAttackRun)
        {
            mbOnAttackRun = false;
            CurDir = mDir;
            Jump();
            mAddSpeed = mAddSpeed_;
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

    protected override void ResetAttack()
    {
        base.ResetAttack();
        mbAttacking = false;
        mAddSpeed = 0;
    }
    override public void OnGroundEnter()
    {
        base.OnGroundEnter();
        mbAttacking = false;
        mAddSpeed = 0;
    }
}