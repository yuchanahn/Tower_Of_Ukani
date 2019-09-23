using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBlackBoard : BlackBoard_MobBase
{
    public float RandomTime = 2f;
    internal bool TA_AttackRun() => ((Mob_Slime)mob).AttackRun();
    override internal bool CN_InAttackAble() => ((Mob_Slime)mob).IsAttacking ? true : mob.StartAttacking = mob.InAttackRange;
}