using ALogic;
using Dongjun.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBatBlackBorad : BlackBoard_Base
{
    private Mob_FlowerBat mMob;
    private StatusEffect_Object status;
    private FlowerBat_Task_RandomMove RandomMoveTask;
    private FlowerBat_Task_Attack AttackTask;
    private FlowerBat_Task_Hang HangTask;
    private FlyingMob_JPS_Task_Follow JPS_FollowTask;

    private void Awake()
    {
        mMob = GetComponent<Mob_FlowerBat>();
        status = GetComponent<StatusEffect_Object>();
        RandomMoveTask = GetComponent<FlowerBat_Task_RandomMove>();
        HangTask = GetComponent<FlowerBat_Task_Hang>();
        JPS_FollowTask = GetComponent<FlyingMob_JPS_Task_Follow>();
        AttackTask = GetComponent<FlowerBat_Task_Attack>();
    }

    public bool IsFindCeiling()
    {
        if(HangTask.IsCeilingNearOf())
        {
            if (HangTask.MyCeiling != null)
                FlowerBat_Task_Hang.HangWalls.Remove(HangTask.MyCeiling.gameObject);
            goto NewFind;
        }

        if (HangTask.FindCeiling)
        {
            HangTask.IsFollowEndToCeiling = HangTask.IsCeilingNear();
            return true;
        }

        NewFind:
        var Tr = Detect.GetHitTrOrNull(transform.position, Vector2.up, HangTask.mCeilingDetectRange, GM.SoildGroundLayer);
        if (HangTask.FindCeiling = HangTask.MyCeiling = Tr)
        {
            if (FlowerBat_Task_Hang.HangWalls.Contains(HangTask.MyCeiling.gameObject))
            {
                return false;
            }
            HangTask.mCeilingPos = Tr.position.Foot(Vector2.one).Add(y: -mMob.Size.y / 2);
        }
        else
        {
            return false;
        }

        FlowerBat_Task_Hang.HangWalls.Add(Tr.gameObject);
        HangTask.IsFollowEndToCeiling = HangTask.IsCeilingNear();

        return true;
    }



    public bool AgroCheck() => Vector2.Distance(transform.position, GM.PlayerPos) <= mMob.AgroRange;
    public bool Follow() => JPS_FollowTask.Tick();






    public bool IsTargetInFleeRange() => false;
    public bool IsTargetInAttackRange()
    {
        AttackTask.AddAttackCoolTimeT();
        return Vector2.Distance(transform.position, GM.PlayerPos) <= mMob.AttackRange;
    }
    public bool BTStop() => status.SENoTask || mMob.BTStop;
    public bool RandomMove() => RandomMoveTask.Tick();
    public bool Hang() => HangTask.Tick();
    public bool Attack() => AttackTask.Tick();
    public bool Flee() => false;
}
