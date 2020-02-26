using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class BT_WeepingMist : BT_Base
{
    WeepingMistBlackBoard Bb;
    override protected void Start()
    {
        Bb = GetComponent<WeepingMistBlackBoard>();
        InitBT(Bb);
    }
    override protected void BT_Set()
    {
        root.node
        .AddNode(new Selector())
            .AddNode(new Decorator(Bb.CN_IsTeleporting))
                .AddNode(new Task(Bb.TA_Teleport))
                .End()
            .End()
            .AddNode(new Decorator(Bb.CN_IsStunned))
                .AddNode(new Task(Bb.TA_StundEvent))
                .End()
            .End()
            .AddNode(new Decorator(Bb.CN_Hurt))
                .AddNode(new Task(Bb.TA_Hurt))
                .End()
            .End()
            .AddNode(new Decorator(Bb.CN_IsPlayerInAttackRange))
                .AddNode(new Task(Bb.TA_Attack))
                .End()
            .End()
            .AddNode(new Decorator(Bb.CN_IsPlayerInAgroRange))
                .AddNode(new Task(Bb.TA_Follow))
                .End()
            .End()


            .AddNode(new Task(Bb.TA_RandomMove))
            .End()
        .End();

        base.BT_Set();
    }
}
