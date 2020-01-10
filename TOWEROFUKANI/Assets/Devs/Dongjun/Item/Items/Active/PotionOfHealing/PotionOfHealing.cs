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
        if (PlayerStats.Health.Value == PlayerStats.Health.Max)
            return;

        cooldownTimer.Restart();

        PlayerStats.Heal(healAmount);
    }
}
