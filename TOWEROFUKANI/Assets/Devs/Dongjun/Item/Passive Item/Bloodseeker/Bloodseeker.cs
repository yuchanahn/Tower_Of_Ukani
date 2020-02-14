using UnityEngine;

public class Bloodseeker : PassiveItem
{
    private ActionEffect onWeaponHitEffect;

    #region Method Override: Add/Remove
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        onWeaponHitEffect = this.CreateActionEffect(LifeSteal);

        ActionEffectManager.AddEffect(PlayerActions.WeaponHit, onWeaponHitEffect);
    }
    public override void OnDrop()
    {
        base.OnDrop();

        ActionEffectManager.RemoveEffect(PlayerActions.WeaponHit, onWeaponHitEffect);
    }
    #endregion

    #region Method: Item Effect
    private void LifeSteal()
    {
        PlayerStats.Inst.Heal(PlayerStats.Inst.DamageToDeal);
    }
    #endregion
}
