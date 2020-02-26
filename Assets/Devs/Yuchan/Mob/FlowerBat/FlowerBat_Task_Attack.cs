using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBat_Task_Attack : Mob_Task_ProjectileAttack
{
    Mob_FlowerBat mMob;
    public bool IsAttacking { get; set; } = false;

    protected override void Awake()
    {
        base.Awake();
        mMob = GetComponent<Mob_FlowerBat>();
    }

    protected override void OnAttackAble()
    {
        mMob.SetAni(eMobAniST.Attack);
        mMob.mHitImmunity = true;
        IsAttacking = true;
    }
    protected override void OnAttackEnd()
    {
        mMob.mHitImmunity = false;
        IsAttacking = false;
    }


    public override bool Tick()
    {
        Target = GM.Player.transform;
        if (!IsAttacking)
        {
            mMob.SetAni(eMobAniST.Fly);
        }
        if (!base.Tick())
        {
            return false;
        }
        mMob.MovementStop();
        mMob.SpriteDir = mMob.SprDirForPlayer;
        return true;
    }
}
