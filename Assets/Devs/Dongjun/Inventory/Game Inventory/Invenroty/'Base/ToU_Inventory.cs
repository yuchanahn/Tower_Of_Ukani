using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToU_Inventory : InventoryBase
{
    #region Method Override: Inventory
    public override void DropItem(int index)
    {
        items[index].OnDrop();
        DeleteItem(index, false);
        inventoryUI?.UpdateSlot(index);
    }
    public override void DropItem(int index, int amount)
    {
        amount = Mathf.Clamp(amount, 0, GetItem(index).Info.Count);
        RemoveItem(index, amount);
        ItemDB.Inst.SpawnDroppedItem(items[index], GM.PlayerPos, amount);
    }
    #endregion

    #region Method: Upgrade Item
    public bool TryUpgradeItem(string itemName)
    {
        int index = GetIndex_ItemName(itemName);
        if (index == -1)
            return false;

        UpgradableItem upgradable = items[index] as UpgradableItem;
        if (upgradable == null)
            return false;

        upgradable.AddLevel();
        return true;
    }
    #endregion
}
