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
    public override void OnDrop()
    {
        base.OnDrop();

        ActionEffectManager.RemoveEffect(PlayerActions.DamageDealt, onHit);
    }
}
