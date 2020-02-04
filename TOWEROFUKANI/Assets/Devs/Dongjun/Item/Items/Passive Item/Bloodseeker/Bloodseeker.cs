using UnityEngine;

public class Bloodseeker : PassiveItem
{
    private ItemEffect onWeaponHitEffect;

    #region Method Override: Add/Remove
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        onWeaponHitEffect = this.CreateItemEffect(LifeSteal);

        ItemEffectManager.AddEffect(PlayerActions.WeaponHit, onWeaponHitEffect);
    }
    public override void OnDrop()
    {
        base.OnDrop();

        ItemEffectManager.RemoveEffect(PlayerActions.WeaponHit, onWeaponHitEffect);
    }
    #endregion

    #region Method: Item Effect
    private void LifeSteal()
    {
        PlayerStats.Inst.Heal(PlayerStats.Inst.DamageToDeal);
    }
    #endregion
}
