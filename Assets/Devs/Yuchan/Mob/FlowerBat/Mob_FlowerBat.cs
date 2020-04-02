using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_FlowerBat : FlyingMob_Base
{
    public override CreatureType CreatureType => CreatureType.Wildlife;
    [SerializeField] public Vector2 Size;
    [SerializeField] FlowerBat_Task_Flee FleeTask;
    [SerializeField] FlowerBat_Task_Hang HangTask;


    protected override void Awake()
    {
        base.Awake();
        FleeTask = GetComponent<FlowerBat_Task_Flee>();
        HangTask = GetComponent<FlowerBat_Task_Hang>();
    }

    //=================================================================
    //      ## Mob_FlowerBat :: Stunned
    //=================================================================

    public override bool Stunned()
    {
        base.Stunned();
        GetComponent<BoxCollider2D>().size = Size;
        return true;
    }

    //=================================================================
    //      ## Mob_FlowerBat :: SetAni
    //=================================================================

    protected override void SetStatusEffectUseAniState()
    {
        base.SetStatusEffectUseAniState();
        bUnHang = false;
        GetComponent<BoxCollider2D>().size = Size;
    }

    void UnHangEnd_AniEvent()
    {
        bUnHang = false;
        mHitImmunity = false;
    }


    bool bUnHang = false;
    eMobAniST mPrevAni = eMobAniST.Last;
    public override void SetAni(eMobAniST ani)
    {
        if(mPrevAni != ani)
        {
            if(mPrevAni == eMobAniST.Hang)
            {
                base.SetAni(eMobAniST.Unhang);

                if (HangTask.MyCeiling != null)
                {
                    FlowerBat_Task_Hang.HangWalls.Remove(HangTask.MyCeiling.gameObject);
                }

                GetComponent<BoxCollider2D>().size = Size;
                bUnHang = true;
                mHitImmunity = true;
                mPrevAni = ani;
                return;
            }
            mPrevAni = ani;
        }
        if(bUnHang)
        {
            Dir2d = Vector2.zero;
            mPrevAni = eMobAniST.Unhang;
            base.SetAni(eMobAniST.Unhang);
        }
        else
        {
            base.SetAni(ani);
        }
    }

    //=================================================================
    //      ## Mob_FlowerBat :: FixedUpdate
    //=================================================================

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(GetComponent<StatusEffect_Knokback>())
        {
            FleeTask.FleeCollider2d.enabled = true;
        }
    }



    //=================================================================
    //      ## Mob_FlowerBat :: Hurt
    //=================================================================

    public override void OnHurt()
    {
        FleeTask.IsFleeAble = Vector2.Distance(transform.position, GM.PlayerPos) <= FleeTask.FleeRange;
        HangTask.WakeAround();

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
        GetComponent<CorpseSpawner>().Spawn();
    }

    public override void OnSuccessfulAttack()
    {
        throw new System.NotImplementedException();
    }
}
