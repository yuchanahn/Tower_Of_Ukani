using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableHotbarUI : InventoryUIBase
{
    [Header("Item UI")]
    [SerializeField] private ConsumableItemUI consumableItemUI;

    protected override ItemUI SpawnItemUI(Item item, int slotIndex)
    {
        ItemUI itemUI = Instantiate(consumableItemUI.gameObject).GetComponent<ItemUI>();

        if (itemUI != null)
            itemUI.Init(item.Info.ItemName, slotUIs[slotIndex].transform, rightClickManager);

        return itemUI;
    }
}
