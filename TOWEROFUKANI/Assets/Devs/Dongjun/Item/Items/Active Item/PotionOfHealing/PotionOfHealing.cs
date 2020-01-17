using UnityEngine;

public class PotionOfHealing : ActiveItem
{
    private int healAmount = 30;

    public override void InitStats()
    {
        cooldownTimer.EndTime = 15f;
    }

    protected override void OnActivate()
    {
        // Don't Activate When Full Health
        if (PlayerStats.Inst.Health.Value == PlayerStats.Inst.Health.Max)
            goto END;

        // Restart Cooldown Timer
        cooldownTimer.Restart();

        // Heal Player
        PlayerStats.Inst.Heal(healAmount);

    // Deactivate Item
    END:
        Deactivate();
    }
}
