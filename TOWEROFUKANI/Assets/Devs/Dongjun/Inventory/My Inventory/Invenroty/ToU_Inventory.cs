using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToU_Inventory : InventoryBase
{
    public override void DropItem(int index)
    {
        items[index].OnDrop();
        DeleteItem(index, false);
        inventoryUI?.UpdateSlot(index);
    }
    public override void DropItem(int index, int amount)
    {
        // ※ 주의: 개별 스탯이 없는 스택 가능한 아이템만 이 방식으로 드롭 해야함!!! 

        amount = Math.Max(0, amount);
        RemoveItem(index, amount);
        ItemDB.Inst.SpawnDroppedItem(items[index].Info.ItemName, amount);
    }
}
