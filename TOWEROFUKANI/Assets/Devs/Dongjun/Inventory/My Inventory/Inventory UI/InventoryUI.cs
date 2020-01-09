using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : InventoryUIBase
{
    [Header("Item UI")]
    [SerializeField] private ItemUI weaponItemUI;
    [SerializeField] private ItemUI activeItemUI;

    protected override ItemUI SpawnItemUI(Item item, int slotIndex)
    {
        ItemUI itemUI = null;

        switch (item)
        {
            case WeaponItem _:
                itemUI = Instantiate(weaponItemUI.gameObject).GetComponent<ItemUI>();
                break;
            case ActiveItem _:
                itemUI = Instantiate(activeItemUI.gameObject).GetComponent<ItemUI>();
                break;
            default:
                break;
        }

        if (itemUI != null)
            itemUI.Init(item.Info.ItemName, slotUIs[slotIndex].transform, rightClickManager);

        return itemUI;
    }
}
