using UnityEngine;

public class DroppedWeapon : DroppedItem
{
    public override void OnPickUp()
    {
        // Get Item Reference
        WeaponItem weaponItem = Item as WeaponItem;

        // Spawn Weapon
        if (!DroppedFromInventory)
            weaponItem = Instantiate(Item).GetComponent<WeaponItem>();

        // Add Existing Weapon
        if (WeaponHotbar.AddExisting(weaponItem))
        {
            Destroy(weaponItem.gameObject);
            Destroy(gameObject);
            return;
        }

        // Add to Hotbar
        if (WeaponHotbar.Add(weaponItem))
        {
            weaponItem.gameObject.SetActive(true);
            weaponItem.transform.SetParent(GM.PlayerObj.transform);
            weaponItem.transform.localPosition = new Vector2(0, weaponItem.PivotPointY);
            Destroy(gameObject);
            return;
        }

        // Add to Inventory
        if (Inventory.Add(weaponItem))
        {
            Destroy(gameObject);
            return;
        }
    }
}
