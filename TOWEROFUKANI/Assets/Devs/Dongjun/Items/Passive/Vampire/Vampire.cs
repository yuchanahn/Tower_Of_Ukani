using UnityEngine;

public class Vampire : PassiveItem
{
    #region Var: Item Effect
    private ItemEffect onHitEffect;
    #endregion

    #region Method Override: Add/Remove
    public override void OnAdd()
    {
        base.OnAdd();

        onHitEffect = new ItemEffect(typeof(Vampire), LifeSteal, typeof(WindCutter));
        ItemEffectManager.AddEffect(PlayerActions.Hit, onHitEffect);
    }
    public override void OnRemove()
    {
        base.OnRemove();

        ItemEffectManager.RemoveEffect(PlayerActions.Hit, onHitEffect);
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
        //Debug.Log("Vampire Activated !!!");
        PlayerStats.Heal(PlayerStats.DamageDealt);
    }
    #endregion
}
