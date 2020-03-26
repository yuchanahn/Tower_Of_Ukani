using UnityEngine;

public class Exzodia : PassiveItem
{
    private PlayerActionEvent onHit;

    public override void InitStats()
    {

    }
    protected override void InitEvents()
    {
        onHit = this.NewPlayerActionEvent(() => PlayerStats.Inst.DamageToDeal = 99999);
    }

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);
        PlayerActionEventManager.AddEvent(PlayerActions.DamageDealt, onHit);
    }
    protected override void OnRemovedFromInventory()
    {
        PlayerActionEventManager.RemoveEvent(PlayerActions.DamageDealt, onHit);
    }
}
