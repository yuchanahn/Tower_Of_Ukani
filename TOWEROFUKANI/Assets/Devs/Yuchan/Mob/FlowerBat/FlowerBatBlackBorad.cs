using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBatBlackBorad : BlackBoard_Base
{
    private Mob_FlowerBat mob;
    private StatusEffect_Object status;
    private FlowerBat_Task_RandomMove RandomMoveTask;
    private FlowerBat_Task_Attack AttackTask;

    private void Awake()
    {
        mob = GetComponent<Mob_FlowerBat>();
        status = GetComponent<StatusEffect_Object>();
        RandomMoveTask = GetComponent<FlowerBat_Task_RandomMove>();
    }

    public bool IsTargetInFleeRange() => false;
    public bool AgroCheck() => false;
    public bool IsTargetInAttackRange() => false;
    public bool StatusEffectBTStop() => status.SENoTask;
    public bool BTStop() => false;
    public bool RandomMove() => RandomMoveTask.Tick();
    public bool Attack() => AttackTask.Tick();
    public bool Flee() => false;
}
