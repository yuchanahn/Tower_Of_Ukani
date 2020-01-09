using UnityEngine;

public class DroppedWeapon : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // Get Item Reference
        WeaponItem weaponItem = ItemObj as WeaponItem;

        // Spawn Item
        if (!DroppedFromInventory)
            weaponItem = Instantiate(ItemObj).GetComponent<WeaponItem>();

        // Add To Inventory
        if (data.weaponHotbar.TryAddItem(weaponItem, data.weaponHotbar.CurSlot))
        {
            Destroy(gameObject);
            return;
        }
        if (data.weaponHotbar.TryAddItem(weaponItem))
        {
            Destroy(gameObject);
            return;
        }
        if (data.inventory.TryAddItem(weaponItem))
        {
            Destroy(gameObject);
            return;
        }
    }
}
