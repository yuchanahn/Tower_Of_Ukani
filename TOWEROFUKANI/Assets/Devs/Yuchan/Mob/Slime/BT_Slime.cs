using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class BT_Slime : BT_Base
{
    SlimeBlackBoard Bb;
    override protected void Start() 
    {
        Bb = GetComponent<SlimeBlackBoard>();
        InitBT(Bb);
    }
    override protected void BT_Set()
    {
        root.node
        .AddNode(new Service(Bb.SV_AgroCheck, 0.1f))
        .AddNode(new Service(Bb.SV_SetRandomDir, 2f))
        .AddNode(new Selector())

            .AddNode(new Decorator(Bb.CN_InHurt))
                .AddNode(new Task(Bb.TA_Hurt))      .TAEnd()
            .End()
            .AddNode(new Decorator(Bb.CN_InAttackAble))
                .AddNode(new Task(Bb.TA_Attack))    .TAEnd()
                .AddNode(new Task(Bb.TA_AttackRun)) .TAEnd()
            .End()

            .AddNode(new Decorator(Bb.CN_InFollowRange))
                .AddNode(new Task(Bb.TA_Follow))    .TAEnd()
            .End() 

            .AddNode(new Task(Bb.TA_RandomMove))
            .End()

        .End()
        .SVEnd()
        .SVEnd();

        base.BT_Set();
    }

}