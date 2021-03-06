﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : InventoryUIBase
{
    [Header("Item UI")]
    [SerializeField] private WeaponItemUI weaponItemUI;
    [SerializeField] private ActiveItemSimpleUI activeItemUI;
    [SerializeField] private ConsumableItemUI consumableItemUI;
    [SerializeField] private ResourceItemUI resourceItemUI;

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
            case ConsumableItem _:
                itemUI = Instantiate(consumableItemUI.gameObject).GetComponent<ItemUI>();
                break;
            case ResourceItem _:
                itemUI = Instantiate(resourceItemUI.gameObject).GetComponent<ItemUI>();
                break;
        }

        if (itemUI != null)
            itemUI.Init(item.Info.ItemName, slotUIs[slotIndex].transform, rightClickAction);

        return itemUI;
    }
}
