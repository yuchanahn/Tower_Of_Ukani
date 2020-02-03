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
        if (data.inventory.TryUpgradeItem(activeItem.Info.ItemName, data.passiveInventory)
        || data.weaponHotbar.TryUpgradeItem(activeItem.Info.ItemName, data.passiveInventory))
            goto EXIT;

        if (data.activeHotbar.TryAddItem(activeItem))
            goto EXIT;

        if (data.inventory.TryAddItem(activeItem))
            goto EXIT;

        return;

    EXIT:
        Destroy(gameObject);
    }
}
