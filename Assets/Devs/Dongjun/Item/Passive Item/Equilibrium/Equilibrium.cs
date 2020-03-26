using UnityEngine;

public class Equilibrium : PassiveItem
{
    #region Var: Item Effect
    private PlayerActionEvent onDealDamage;
    private PlayerActionEvent onDamaged;
    #endregion

    #region Var: Item Stats
    private float effectPercent = 30f;
    #endregion

    #region Method: Item
    public override void InitStats()
    {

    }
    protected override void InitEvents()
    {
        onDealDamage = this.NewPlayerActionEvent(() =>
        {
            PlayerStats.Inst.DamageToDeal += MathD.Round(PlayerStats.Inst.DamageToDeal * (effectPercent * 0.01f));
        });

        onDamaged = this.NewPlayerActionEvent(() =>
        {
            PlayerStats.Inst.DamageToReceive += MathD.Round(PlayerStats.Inst.DamageToReceive * (effectPercent * 0.01f));
        });
    }

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        PlayerActionEventManager.AddEvent(PlayerActions.HealthDamaged, onDamaged);
        PlayerActionEventManager.AddEvent(PlayerActions.DamageDealt, onDealDamage);
    }
    protected override void OnRemovedFromInventory()
    {
        PlayerActionEventManager.RemoveEvent(PlayerActions.HealthDamaged, onDamaged);
        PlayerActionEventManager.RemoveEvent(PlayerActions.DamageDealt, onDealDamage);
    }
    #endregion
}
