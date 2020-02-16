using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBat_Task_Attack : Mob_Task_ProjectileAttack
{
    Mob_FlowerBat mob;

    protected override void Awake()
    {
        base.Awake();
        mob = GetComponent<Mob_FlowerBat>();
    }

    protected override void OnAttackAble()
    {
        mob.SetAni(eMobAniST.Attack);
        mob.mHitImmunity = true;
    }
    protected override void OnAttackEnd()
    {
        mob.SetAni(eMobAniST.Fly);
        mob.mHitImmunity = false;
    }


    public override bool Tick()
    {
        Target = GM.Player.transform;
        if(!base.Tick())
        {
            return false;
        }
        mob.Dir2d = Vector2.zero;
        mob.SpriteDir = mob.SprDirForPlayer;
        return true;
    }
}
