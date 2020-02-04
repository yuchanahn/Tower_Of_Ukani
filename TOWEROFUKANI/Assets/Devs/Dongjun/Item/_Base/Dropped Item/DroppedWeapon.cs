using Dongjun.Helper;
using System.Collections;
using UnityEngine;

public class DroppedWeapon : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        WeaponItem weaponItem = Item as WeaponItem;

        // Spawn Item
        if (!DroppedFromInventory)
            weaponItem = Instantiate(Item).GetComponent<WeaponItem>();

        // Add To Inventory
        if (PlayerInventoryManager.inventory.TryUpgradeItem(weaponItem.Info.ItemName)
        || PlayerInventoryManager.weaponHotbar.TryUpgradeItem(weaponItem.Info.ItemName))
        {
            Destroy(weaponItem.gameObject);
            goto EXIT;
        }

        if (PlayerInventoryManager.weaponHotbar.TryAddItem(weaponItem, PlayerInventoryManager.weaponHotbar.CurSlot))
            goto EXIT;

        if (PlayerInventoryManager.weaponHotbar.TryAddItem(weaponItem))
            goto EXIT;

        if (PlayerInventoryManager.inventory.TryAddItem(weaponItem))
            goto EXIT;

        return;

    EXIT:
        Destroy(gameObject);
    }
}
