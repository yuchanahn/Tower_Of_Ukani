﻿using UnityEngine;

public class Equilibrium : PassiveItem
{
    private ActionEffect onDamagedEffect;
    private ActionEffect onHitEffect;

    private float effectPercent = 30f;

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        onDamagedEffect = this.CreateActionEffect(OnDamaged);
        onHitEffect = this.CreateActionEffect(OnHit);

        ActionEffectManager.AddEffect(PlayerActions.Damaged, onDamagedEffect);
        ActionEffectManager.AddEffect(PlayerActions.DamageDealt, onHitEffect);
    }
    protected override void OnRemovedFromInventory()
    {
        ActionEffectManager.RemoveEffect(PlayerActions.Damaged, onDamagedEffect);
        ActionEffectManager.RemoveEffect(PlayerActions.DamageDealt, onHitEffect);
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
