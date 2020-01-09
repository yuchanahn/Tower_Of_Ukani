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
        if (data.activeHotbar.TryAddItem(activeItem))
        {
            Destroy(gameObject);
            return;
        }
        if (data.inventory.TryAddItem(activeItem))
        {
            Destroy(gameObject);
            return;
        }
    }
}
