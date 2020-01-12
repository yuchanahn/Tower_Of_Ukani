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
        if (data.passiveInventory.TryUpgradeItem(passiveItem,
            data.inventory,
            data.weaponHotbar))
        {
            Destroy(passiveItem.gameObject);
            goto EXIT;
        }
        if (data.passiveInventory.TryAddItem(passiveItem))
        {
            goto EXIT;
        }
        return;

    EXIT:
        Destroy(gameObject);
        return;
    }
}
