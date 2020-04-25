using Dongjun.Helper;
using UnityEngine;

public class DroppedWeapon : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        WeaponItem weaponItem = Item as WeaponItem;

        // Spawn Item
        if (Item.gameObject.IsPrefab())
            weaponItem = ItemDB.Inst.SpawnItem(Item.GetType()).GetComponent<WeaponItem>();

        // Add To Inventory
        if (PlayerInventoryManager.inventory.TryUpgradeItem(weaponItem.GetType())
        || PlayerInventoryManager.weaponHotbar.TryUpgradeItem(weaponItem.GetType()))
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
