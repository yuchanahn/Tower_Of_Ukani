using Dongjun.Helper;
using UnityEngine;

public class DroppedActiveItem : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        ActiveItem activeItem = Item as ActiveItem;

        // Spawn Item
        if (Item.gameObject.IsPrefab())
            activeItem = ItemDB.Inst.SpawnItem(Item.GetType()).GetComponent<ActiveItem>();

        // Add To Inventory
        if (PlayerInventoryManager.inventory.TryUpgradeItem(activeItem.GetType())
        || PlayerInventoryManager.activeHotbar.TryUpgradeItem(activeItem.GetType()))
        {
            Destroy(activeItem.gameObject);
            goto EXIT;
        }

        if (PlayerInventoryManager.activeHotbar.TryAddItem(activeItem))
            goto EXIT;

        if (PlayerInventoryManager.inventory.TryAddItem(activeItem))
            goto EXIT;

        return;

    EXIT:
        Destroy(gameObject);
    }
}
