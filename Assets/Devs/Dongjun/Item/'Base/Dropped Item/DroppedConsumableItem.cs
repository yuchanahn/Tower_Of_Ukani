using Dongjun.Helper;
using UnityEngine;

public class DroppedConsumableItem : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        ConsumableItem consumableItem = Item as ConsumableItem;

        // Spawn Item
        if (Item.gameObject.IsPrefab())
            consumableItem = ItemDB.Inst.SpawnItem(Item.GetType()).GetComponent<ConsumableItem>();

        // Add To Inventory
        if (PlayerInventoryManager.consumableHotbar.TryAddItem(consumableItem))
            goto EXIT;

        if (PlayerInventoryManager.inventory.TryAddItem(consumableItem))
            goto EXIT;

        return;

    EXIT:
        Destroy(gameObject);
    }
}
