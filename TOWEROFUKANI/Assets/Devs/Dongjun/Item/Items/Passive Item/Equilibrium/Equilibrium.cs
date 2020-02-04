using UnityEngine;

public class Equilibrium : PassiveItem
{
    private ItemEffect onDamagedEffect;
    private ItemEffect onHitEffect;

    private float effectPercent = 30f;

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        onDamagedEffect = this.CreateItemEffect(OnDamaged);
        onHitEffect = this.CreateItemEffect(OnHit);

        ItemEffectManager.AddEffect(PlayerActions.Damaged, onDamagedEffect);
        ItemEffectManager.AddEffect(PlayerActions.DamageDealt, onHitEffect);
    }
    public override void OnDrop()
    {
        base.OnDrop();
        ItemEffectManager.RemoveEffect(PlayerActions.Damaged, onDamagedEffect);
        ItemEffectManager.RemoveEffect(PlayerActions.DamageDealt, onHitEffect);
    }

    private void OnDamaged()
    {
        PlayerStats.Inst.DamageReceived += MathD.Round(PlayerStats.Inst.DamageReceived * (effectPercent * 0.01f));
    }
    private void OnHit()
    {
        PlayerStats.Inst.DamageToDeal += MathD.Round(PlayerStats.Inst.DamageToDeal * (effectPercent * 0.01f));
    }
}
