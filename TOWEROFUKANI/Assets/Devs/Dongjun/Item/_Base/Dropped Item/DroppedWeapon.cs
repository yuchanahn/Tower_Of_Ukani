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
        if (data.inventory.TryUpgradeItem(weaponItem.Info.ItemName, data.passiveInventory)
        ||  data.weaponHotbar.TryUpgradeItem(weaponItem.Info.ItemName, data.passiveInventory))
        {
            Destroy(weaponItem.gameObject);
            goto EXIT;
        }
        if (data.weaponHotbar.TryAddItem(weaponItem, data.weaponHotbar.CurSlot))
        {
            goto EXIT;
        }
        if (data.weaponHotbar.TryAddItem(weaponItem))
        {
            goto EXIT;
        }
        if (data.inventory.TryAddItem(weaponItem))
        {
            goto EXIT;
        }
        return;

    EXIT:
        Destroy(gameObject);
        return;
    }
}
