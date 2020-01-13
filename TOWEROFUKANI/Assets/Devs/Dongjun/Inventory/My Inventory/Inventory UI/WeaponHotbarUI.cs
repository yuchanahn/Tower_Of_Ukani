using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHotbarUI : InventoryUIBase
{
    [Header("Item UI")]
    [SerializeField] private WeaponItemUI weaponItemUI;

    protected override ItemUI SpawnItemUI(Item item, int slotIndex)
    {
        ItemUI itemUI = Instantiate(weaponItemUI.gameObject).GetComponent<ItemUI>();

        if (itemUI != null)
            itemUI.Init(item.Info.ItemName, slotUIs[slotIndex].transform, rightClickManager);

        return itemUI;
    }
}
