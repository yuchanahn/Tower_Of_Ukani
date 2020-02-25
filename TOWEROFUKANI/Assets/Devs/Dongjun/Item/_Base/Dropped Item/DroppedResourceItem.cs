using Dongjun.Helper;
using UnityEngine;

public class DroppedResourceItem : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        ResourceItem resourceItem = Item as ResourceItem;

        // Spawn Item
        if (Item.gameObject.IsPrefab())
            resourceItem = ItemDB.Inst.SpawnItem(Item.Info.ItemName).GetComponent<ResourceItem>();

        // Add To Inventory
        if (PlayerInventoryManager.inventory.TryAddItem(resourceItem))
            goto EXIT;

        return;

    EXIT:
        Destroy(gameObject);
    }
}
