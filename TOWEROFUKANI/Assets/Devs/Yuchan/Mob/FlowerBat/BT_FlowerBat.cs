using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class BT_FlowerBat : BT_Base
{
    FlowerBatBlackBorad Bb;
    override protected void Start()
    {
        Bb = GetComponent<FlowerBatBlackBorad>();
        InitBT(Bb);
    }
    override protected void BT_Set()
    {
        root.node
        .AddNode(new Selector())
            .AddNode(new Decorator(Bb.BTStop))
                .AddNode(new Task(() => true))
                .End()
            .End()
            .AddNode(new Decorator(Bb.IsTargetInFleeRange))
                .AddNode(new Task(Bb.Flee))
                .End()
            .End()
            .AddNode(new Decorator(Bb.IsTargetInAttackRange))
                .AddNode(new Task(Bb.Attack))
                .End()
            .End()
            .AddNode(new Decorator(Bb.AgroCheck))
                .AddNode(new Task(Bb.Follow))
                .End()
            .End()
            .AddNode(new Decorator(Bb.IsFindCeiling))
                .AddNode(new Task(Bb.Hang))
                .End()
            .End()
            .AddNode(new Task(Bb.RandomMove))
            .End()
        .End();

        base.BT_Set();
    }

}