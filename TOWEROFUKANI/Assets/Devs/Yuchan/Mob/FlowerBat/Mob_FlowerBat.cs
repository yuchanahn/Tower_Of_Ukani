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
        if (mHitImmunity) return;
        BTStop = true;
        base.OnHurt();
    }

    protected override void HurtEnd()
    {
        base.HurtEnd();
        SetAni(eMobAniST.Fly);
        BTStop = false;
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
