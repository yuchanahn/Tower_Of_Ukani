﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedConsumableItem : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        ConsumableItem consumableItem = Item as ConsumableItem;

        // Spawn Item
        if (!DroppedFromInventory)
            consumableItem = Instantiate(consumableItem).GetComponent<ConsumableItem>();

        // Add To Inventory
        if (data.consumableHotbar.TryAddItem(consumableItem))
        {
            goto EXIT;
        }
        if (data.inventory.TryAddItem(consumableItem))
        {
            goto EXIT;
        }
        return;

    EXIT:
        Destroy(gameObject);
        return;
    }
}
