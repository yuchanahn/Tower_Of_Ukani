using UnityEngine;

public class DroppedActiveItem : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        ActiveItem activeItem = ItemObj as ActiveItem;

        // Spawn Item
        if (!DroppedFromInventory)
            activeItem = Instantiate(activeItem).GetComponent<ActiveItem>();

        // Add To Inventory
        if (data.inventory.TryUpgradeItem(activeItem.Info.ItemName)
        || data.weaponHotbar.TryUpgradeItem(activeItem.Info.ItemName))
        {
            goto EXIT;
        }
        if (data.activeHotbar.TryAddItem(activeItem))
        {
            goto EXIT;
        }
        if (data.inventory.TryAddItem(activeItem))
        {
            goto EXIT;
        }

    EXIT:
        Destroy(gameObject);
        return;
    }
}
