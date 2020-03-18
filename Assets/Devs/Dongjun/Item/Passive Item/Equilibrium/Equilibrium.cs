using UnityEngine;

public class Equilibrium : PassiveItem
{
    #region Var: Player Action Event
    private PlayerActionEvent onDealDamage;
    private PlayerActionEvent onDamaged;
    #endregion

    #region Var: Stat
    private float effectPercent = 30f;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        onDealDamage = this.NewPlayerActionEvent(() =>
        {
            PlayerStats.Inst.DamageToDeal += MathD.Round(PlayerStats.Inst.DamageToDeal * (effectPercent * 0.01f));
        });
        onDamaged = this.NewPlayerActionEvent(() =>
        {
            PlayerStats.Inst.DamageReceived += MathD.Round(PlayerStats.Inst.DamageReceived * (effectPercent * 0.01f));
        });
    }
    #endregion

    #region Method: Item
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
