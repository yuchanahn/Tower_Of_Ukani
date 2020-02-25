using UnityEngine;

public class DroppedActiveItem : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        ActiveItem activeItem = Item as ActiveItem;

        // Spawn Item
        if (!DroppedFromInventory)
            activeItem = Instantiate(activeItem).GetComponent<ActiveItem>();

        // Add To Inventory
        if (PlayerInventoryManager.inventory.TryUpgradeItem(activeItem.Info.ItemName)
        || PlayerInventoryManager.activeHotbar.TryUpgradeItem(activeItem.Info.ItemName))
            goto EXIT;

        if (PlayerInventoryManager.activeHotbar.TryAddItem(activeItem))
            goto EXIT;

        if (PlayerInventoryManager.inventory.TryAddItem(activeItem))
            goto EXIT;

        return;

    EXIT:
        Destroy(gameObject);
    }
}
