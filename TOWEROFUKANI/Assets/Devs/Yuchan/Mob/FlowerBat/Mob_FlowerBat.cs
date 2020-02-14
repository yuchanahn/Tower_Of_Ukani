using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_FlowerBat : FlyingMob_Base
{
    public override CreatureType CreatureType => CreatureType.Wildlife;

    //=================================================================
    //      ## Flee : Mob_FlowerBat
    //=================================================================

    [SerializeField] float FleeRange;
    [SerializeField] public Vector2 Size;

    //=================================================================
    //      ## Attack : override
    //=================================================================


    override protected void PreAttack()
    {
        mCurAniST = eMobAniST.Attack_Post;
    }
    protected override void Attack()
    {
        mCurAniST = eMobAniST.Cry;
    }
    protected override void OnAttackEnd()
    {

    }

    protected override void OnAttackStart()
    {
        base.OnAttackStart();
    }
    private void OnDestroy()
    {
        ATimer.Pop(GetInstanceID() + "AttackEndTimer");
    }

    //=================================================================
    //      ## Mob_FlowerBat :: Stunned
    //=================================================================

    public override bool Stunned()
    {
        base.Stunned();
        return true;
    }

    //=================================================================
    //      ## Mob_FlowerBat :: Hurt
    //=================================================================

    public override void OnHurt()
    {
        base.OnHurt();
    }

    //=================================================================
    //      ## Mob_FlowerBat :: Dead
    //=================================================================

    public override void OnDead()
    {
        base.OnDead();
    }

    public override void OnSuccessfulAttack()
    {
        throw new System.NotImplementedException();
    }
}
