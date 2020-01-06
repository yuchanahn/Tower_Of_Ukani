using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToU_Inventory : InventoryBase
{
    public override void DropItem(Item item, int amount = 1)
    {
        item.OnRemove();
    }
}
