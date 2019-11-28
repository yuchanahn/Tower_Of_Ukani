using UnityEngine;

public class Equilibrium : PassiveItem
{
    private ItemEffect onDamagedEffect;
    private ItemEffect onHitEffect;

    private float effectPercent = 50f;

    public override void OnAdd()
    {
        onDamagedEffect = new ItemEffect(GetType(), OnDamaged);
        onHitEffect = new ItemEffect(GetType(), OnHit);

        ItemEffectManager.AddEffect(PlayerActions.Damaged, onDamagedEffect);
        ItemEffectManager.AddEffect(PlayerActions.Hit, onHitEffect);
    }
    public override void OnRemove()
    {
        base.OnRemove();
        ItemEffectManager.RemoveEffect(PlayerActions.Damaged, onDamagedEffect);
        ItemEffectManager.RemoveEffect(PlayerActions.Hit, onHitEffect);
    }

    protected override void SetBonusStats(WeaponItem weapon)
    {

    }

    private void OnDamaged()
    {
        PlayerStats.DamageReceived += MathD.Round(PlayerStats.DamageReceived * (effectPercent * 0.01f));
    }
    private void OnHit()
    {
        PlayerStats.DamageToDeal += MathD.Round(PlayerStats.DamageToDeal * (effectPercent * 0.01f));
    }
}
