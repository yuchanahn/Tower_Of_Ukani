﻿using UnityEngine;

public class WoodenShortbowItem : BowItem
{
    public override void InitStats()
    {
        // Attack Data
        AttackData = new AttackData(5);

        // Arrow Data
        arrowData = new ProjectileData()
        {
            moveSpeed = new FloatStat(30f, min: 0f),
            travelDist = new FloatStat(min: 0f, max: 30f),
            gravity = new FloatStat(40f, min: 0f)
        };

        // Timer Data
        Timer_Shoot.EndTime = new FloatStat(0.2f, min: 0f);
        Timer_Draw.EndTime = new FloatStat(0.5f, min: 0.01f);
    }
}