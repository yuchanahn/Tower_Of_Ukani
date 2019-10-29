using UnityEngine;

public class PlayerItemDrop_Logic : MonoBehaviour
{
    public static void DropFromHotbar(bool input)
    {
        if (!input || WeaponHolder.Inst.currentWeapon is null)
            return;

        Inventory.Inst.RemoveItem(WeaponHolder.Inst.currentWeapon);
        WeaponHolder.Inst.RemoveWeapon();
    }
    public static void DropFromInventory(Item item)
    {
        if (Inventory.Inst.items.Contains(item))
            return;

        Inventory.Inst.RemoveItem(item);
    }
}
