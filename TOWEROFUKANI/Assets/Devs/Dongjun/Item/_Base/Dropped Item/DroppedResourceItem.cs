using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedResourceItem : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        ResourceItem resourceItem = Item as ResourceItem;

        // Spawn Item
        if (resourceItem.gameObject.IsPrefab())
            resourceItem = Instantiate(resourceItem).GetComponent<ResourceItem>();

        // Add To Inventory
        if (PlayerInventoryManager.inventory.TryAddItem(resourceItem))
            goto EXIT;

        return;

    EXIT:
        Destroy(gameObject);
    }
}
