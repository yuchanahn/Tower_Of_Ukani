using UnityEngine;

public class Bloodseeker : PassiveItem
{
    #region Var: Item Effect
    private ItemEffect onWeaponHitEffect;
    #endregion

    #region Method: Initialize
    public override void InitStats()
    {

    }
    #endregion

    #region Method Override: Add/Remove
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        onWeaponHitEffect = new ItemEffect(GetType(), LifeSteal);
        ItemEffectManager.AddEffect(PlayerActions.WeaponHit, onWeaponHitEffect);
    }
    public override void OnDrop()
    {
        base.OnDrop();
        ItemEffectManager.RemoveEffect(PlayerActions.WeaponHit, onWeaponHitEffect);
    }
    #endregion

    #region Method Override: Bonus Stats
    public override void ApplyBonusStats(WeaponItem weapon)
    {

    }
    #endregion

    #region Method: Item Effect
    private void LifeSteal()
    {
        PlayerStats.Heal(PlayerStats.DamageToDeal);
    }
    #endregion
}
