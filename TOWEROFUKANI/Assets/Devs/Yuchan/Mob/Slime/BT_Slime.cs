using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Slime : BT.BT_Base
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
        .AddNode(new BT.Service(Bb.SV_AgroCheck, 0.1f))
        .AddNode(new BT.Selector())
            .AddNode(new BT.Decorator(Bb.CN_InHurt))
                .AddNode(new BT.Task(Bb.TA_Hurt))
                
                .End()
            .End()
            .AddNode(new BT.Decorator(Bb.CN_InAttackRange))
                .AddNode(new BT.Task(Bb.TA_Attack))
                
                .End()
                .AddNode(new BT.Task(Bb.TA_Run))
                
                .End()
            .End()

            .AddNode(new BT.Decorator(Bb.CN_InFollowRange))
                .AddNode(new BT.Task(Bb.TA_Follow))
                
                .End()
            .End()

            .AddNode(new BT.Task(Bb.CN_RandomMove))
            .End()
        .End()    
        .End();

        base.BT_Set();
    }

}