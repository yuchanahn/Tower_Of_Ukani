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

    private void Awake()
    {
        mMob = GetComponent<Mob_FlowerBat>();
        status = GetComponent<StatusEffect_Object>();
        RandomMoveTask = GetComponent<FlowerBat_Task_RandomMove>();
        HangTask = GetComponent<FlowerBat_Task_Hang>();
    }

    public bool IsFindCeiling()
    {
        if (HangTask.FindCeiling)
        {
            HangTask.IsFollowEndToCeiling = HangTask.IsCeilingNear();
            return true;
        }

        var Tr = Detect.GetHitTrOrNull(transform.position, Vector2.up, HangTask.mCeilingDetectRange, GM.SoildGroundLayer);
        if (HangTask.FindCeiling = Tr)
        {
            if (FlowerBat_Task_Hang.HangWalls.Contains(Tr.gameObject))
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



    public bool AgroCheck() => false;
    public bool Follow() => false;






    public bool IsTargetInFleeRange() => false;
    public bool IsTargetInAttackRange() => false;
    public bool StatusEffectBTStop() => status.SENoTask;
    public bool BTStop() => false;
    public bool RandomMove() => RandomMoveTask.Tick();
    public bool Hang() => HangTask.Tick();
    public bool Attack() => AttackTask.Tick();
    public bool Flee() => false;
}
