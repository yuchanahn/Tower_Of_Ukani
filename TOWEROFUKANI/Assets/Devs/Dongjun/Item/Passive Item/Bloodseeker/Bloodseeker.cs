using UnityEngine;

public class Bloodseeker : PassiveItem
{
    #region Var: Player Action Event
    private PlayerActionEvent onWeaponHit;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        // Player Action Event
        onWeaponHit = this.NewPlayerActionEvent(() =>
        {
            // Heal
            PlayerStats.Inst.Heal(PlayerStats.Inst.DamageToDeal);
        });
    }
    #endregion

    #region Method: Item
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        PlayerActionEventManager.AddEvent(PlayerActions.WeaponHit, onWeaponHit);
    }
    protected override void OnRemovedFromInventory()
    {
        PlayerActionEventManager.RemoveEvent(PlayerActions.WeaponHit, onWeaponHit);
    }
    #endregion
}
