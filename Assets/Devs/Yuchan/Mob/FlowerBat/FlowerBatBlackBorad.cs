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
    private FlowerBat_Task_Flee FleeTask;

    private void Awake()
    {
        mMob                = GetComponent<Mob_FlowerBat>();
        status              = GetComponent<StatusEffect_Object>();
        RandomMoveTask      = GetComponent<FlowerBat_Task_RandomMove>();
        HangTask            = GetComponent<FlowerBat_Task_Hang>();
        JPS_FollowTask      = GetComponent<FlyingMob_JPS_Task_Follow>();
        AttackTask          = GetComponent<FlowerBat_Task_Attack>();
        FleeTask            = GetComponent<FlowerBat_Task_Flee>();
    }
    float FindCeilingT = 0f;
    bool bUseTimer = true;
    public bool IsFindCeiling()
    {
        HangTask.mCeilingCoolTimeT += Time.deltaTime;
        if (!HangTask.IsCeilingNearOf())
        {

        }
        else if (HangTask.FindCeiling)
        {
            HangTask.IsFollowEndToCeiling = HangTask.IsCeilingNear();
            return true;
        }


        if((FindCeilingT += Time.deltaTime) < HangTask.CeilingCoolTime)
        {
            return false;
        }
        // 벽에 매달리지 않는다.
        // 벽을 검색 하지 않는 타이머를 설정한 뒤 빠져나온다.
        if (bUseTimer && !ARandom.Get(HangTask.CeilingPercentage))
        {
            FindCeilingT = 0;
            return false;
        }
        else
        {
            var Tr = Detect.GetHitTrOrNull(transform.position, Vector2.up, HangTask.mCeilingDetectRange, GM.SoildGroundLayer);
            if (Tr)
            {
                HangTask.MyCeiling = Tr;
                // 발견한 천장이 이미 누가 매달려 있는 벽이라면?
                if (FlowerBat_Task_Hang.HangWalls.Contains(HangTask.MyCeiling.gameObject))
                {
                    bUseTimer = false;
                    return false;
                }

                //찾은 천장을 목적지로 설정.
                HangTask.mCeilingPos = Tr.position;
                //천장을 찾았다.
                HangTask.FindCeiling = true;
                bUseTimer = true;
            }
            else
            {
                bUseTimer = false;
                return false;
            }

            FlowerBat_Task_Hang.HangWalls.Add(Tr.gameObject);
            HangTask.IsFollowEndToCeiling = HangTask.IsCeilingNear();
            return true;
        }
    }



    public bool AgroCheck()
    {
        bool r = !HangTask.IsFollowEndToCeiling && Vector2.Distance(transform.position, GM.PlayerPos) <= mMob.AgroRange;
        if(r)
        {
            RandomMoveTask.ReSet();
        }
        return r;
    }
    public bool Follow() => JPS_FollowTask.Tick();



    public bool IsTargetInFleeRange()
    {
        if (FleeTask.IsFleeAble)
        {
            FleeTask.IsFleeing = true;
        }
        return FleeTask.FleeCollider2d.enabled = FleeTask.IsFleeing;
    }
    public bool IsTargetInAttackRange()
    {
        AttackTask.AddAttackCoolTimeT();
        if (AttackTask.IsAttacking)
        {
            return true;
        }
        if (Vector2.Distance(transform.position, GM.PlayerPos) <= mMob.AttackRange)
        {
            return Physics2D.Raycast(transform.position, 
                (GM.PlayerPos - transform.position).normalized, 
                Vector2.Distance(GM.PlayerPos, transform.position), 
                GM.SoildGroundLayer).transform is null && !IsTargetInFleeRange() && !HangTask.IsFollowEndToCeiling;
        }

        return AttackTask.IsAttacking;
    }
    public bool BTStop() => status.SENoTask || mMob.BTStop;
    public bool RandomMove() => RandomMoveTask.Tick();
    public bool Hang() => HangTask.Tick();
    public bool Attack() => AttackTask.Tick();
    public bool Flee() => FleeTask.Tick();
}
