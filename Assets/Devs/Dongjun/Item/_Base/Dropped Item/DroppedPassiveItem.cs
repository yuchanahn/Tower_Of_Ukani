using Dongjun.Helper;
using UnityEngine;

public class DroppedPassiveItem : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        PassiveItem passiveItem = Item as PassiveItem;

        // Spawn Item
        if (Item.gameObject.IsPrefab())
            passiveItem = ItemDB.Inst.SpawnItem(Item.Info.ItemName).GetComponent<PassiveItem>();

        // Add To Inventory
        if (PlayerInventoryManager.passiveInventory.TryUpgradeItem(passiveItem.Info.ItemName))
        {
            Destroy(passiveItem.gameObject);
            goto EXIT;
        }

        if (PlayerInventoryManager.passiveInventory.TryAddItem(passiveItem))
            goto EXIT;

        return;

    EXIT:
        Destroy(gameObject);
    }
}
