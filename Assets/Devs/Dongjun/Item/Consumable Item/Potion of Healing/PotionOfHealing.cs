﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionOfHealing : ConsumableItem
{
    [SerializeField] private float healAmount;

    public override bool Consume()
    {
        // Don't Activate When Full Health
        if (PlayerStats.Inst.health.Value == PlayerStats.Inst.health.Max)
            return false;

        // Heal Player
        PlayerStats.Inst.Heal(healAmount);
        return true;
    }
}
