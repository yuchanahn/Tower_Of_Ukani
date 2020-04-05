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
        
        mMob.mHitImmunity = true;
        IsAttacking = true;
    }
    protected override void OnAttackEnd()
    {
        mMob.mHitImmunity = false;
        IsAttacking = false;
    }

    protected override void Mob_Task_ProjectileAttack_AttackStart_AniEvent()
    {
        var o = Mob_Task_ProjectileAttack_Attack();
        o.GetComponent<Projectile_UltrasonicWave>().Damage = GetComponent<AStat>().Damage;
    }

    public override bool Tick()
    {
        Target = GM.Player.transform;
        mMob.SetAni(IsAttacking ? eMobAniST.Attack : eMobAniST.Fly);
        if (!base.Tick())
        {
            return false;
        }
        mMob.MovementStop();
        mMob.SpriteDir = mMob.SprDirForPlayer;
        return true;
    }
}
