using UnityEngine;

public class Vampire : PassiveItem
{
    ItemEffect onHitEffect = new ItemEffect();

    public override void OnAdd()
    {
        Max_Count = 3;
        onHitEffect.action = LifeSteal;

        ItemEffectManager.AddEffect(PlayerActions.Hit, onHitEffect);
    }

    protected override void SetBonusStats(Item item) { }

    private void LifeSteal()
    {
        PlayerStats.Heal(PlayerStats.DamageDealt);
    }
}
