using UnityEngine;

public class Bloodseeker : PassiveItem
{
    #region Var: Item Effect
    private ItemEffect onWeaponHitEffect;
    #endregion

    #region Method Override: Add/Remove
    public override void OnAdd()
    {
        onWeaponHitEffect = new ItemEffect(GetType(), LifeSteal);
        ItemEffectManager.AddEffect(PlayerActions.WeaponHit, onWeaponHitEffect);
    }
    public override void OnRemove()
    {
        base.OnRemove();
        ItemEffectManager.RemoveEffect(PlayerActions.WeaponHit, onWeaponHitEffect);
    }
    #endregion

    #region Method Override: Bonus Stats
    protected override void SetBonusStats(WeaponItem weapon)
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
