using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionOfHealing : ConsumableItem
{
    [SerializeField] private SelfSleepObj healEffectPrefab;
    [SerializeField] private float healAmount;

    public override bool Consume()
    {
        // Don't Activate When Full Health
        if (PlayerStats.Inst.Health.Value == PlayerStats.Inst.Health.Max)
            return false;

        // Heal Player
        PlayerStats.Inst.Heal(healAmount);

        // Effect
        healEffectPrefab.Spawn(GM.Player.transform, Vector2.zero);
        return true;
    }
}
