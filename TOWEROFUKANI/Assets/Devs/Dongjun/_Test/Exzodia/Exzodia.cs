using UnityEngine;

public class Exzodia : PassiveItem
{
    private ActionEffect onHit;

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        onHit = this.CreateActionEffect(() => PlayerStats.Inst.DamageToDeal = 99999);
        ActionEffectManager.AddEffect(PlayerActions.DamageDealt, onHit);
    }
    protected override void OnRemovedFromInventory()
    {
        ActionEffectManager.RemoveEffect(PlayerActions.DamageDealt, onHit);
    }
}
