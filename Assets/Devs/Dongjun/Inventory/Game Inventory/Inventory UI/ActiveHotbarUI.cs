using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveHotbarUI : InventoryUIBase
{
    [Header("Item UI")]
    [SerializeField] private ActiveItemUI activeItemUI;

    protected override ItemUI SpawnItemUI(Item item, int slotIndex)
    {
        ItemUI itemUI = Instantiate(activeItemUI.gameObject).GetComponent<ItemUI>();

        if (itemUI != null)
            itemUI.Init(item.Info.ItemName, slotUIs[slotIndex].transform, rightClickAction);

        return itemUI;
    }
}
