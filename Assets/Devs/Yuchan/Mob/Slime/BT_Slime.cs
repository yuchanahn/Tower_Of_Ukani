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
        .AddNode(new Selector())
            .AddNode(new Task(Bb.TA_SENoAct))
            .End()
            .AddNode(new Decorator(Bb.CN_IsHurted))
                .AddNode(new Task(Bb.TA_Hurt))
                .End()
            .End()
            .AddNode(new Decorator(Bb.CN_IsAttack))
                .AddNode(new Task(Bb.TA_Attack))
                .End()
            .End()
            .AddNode(new Decorator(Bb.CN_IsFollow))
                .AddNode(new Task(Bb.TA_Follow))
                .End()
            .End()
            .AddNode(new Sequence())
                .AddNode(new Task(Bb.TA_RandMove))
                .End()
                .AddNode(new Task(Bb.TA_Idle))
                .End()
            .End()
            .AddNode(new Task(Bb.TA_Falling))
            .End()
        .End();

        base.BT_Set();
    }

}