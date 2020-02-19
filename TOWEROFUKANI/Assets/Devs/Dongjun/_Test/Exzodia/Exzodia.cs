using UnityEngine;

public class Exzodia : PassiveItem
{
    private PlayerActionEvent onHit;

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        onHit = this.NewPlayerActionEvent(() => PlayerStats.Inst.DamageToDeal = 99999);
        PlayerActionEventManager.AddEvent(PlayerActions.DamageDealt, onHit);
    }
    protected override void OnRemovedFromInventory()
    {
        PlayerActionEventManager.RemoveEvent(PlayerActions.DamageDealt, onHit);
    }
}
