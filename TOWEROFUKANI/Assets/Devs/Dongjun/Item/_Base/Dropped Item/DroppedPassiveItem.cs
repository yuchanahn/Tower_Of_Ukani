using UnityEngine;

public class DroppedPassiveItem : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        PassiveItem passiveItem = Item as PassiveItem;

        // Spawn Item
        if (!DroppedFromInventory)
            passiveItem = Instantiate(passiveItem).GetComponent<PassiveItem>();

        // Add To Inventory
        if (PlayerInventoryManager.passiveInventory.TryUpgradeItem(passiveItem))
        {
            Destroy(passiveItem.gameObject);
            goto EXIT;
        }
        if (PlayerInventoryManager.passiveInventory.TryAddItem(passiveItem))
        {
            goto EXIT;
        }
        return;

    EXIT:
        Destroy(gameObject);
        return;
    }
}
