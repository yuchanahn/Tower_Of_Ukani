using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedPassiveItem : DroppedItem
{
    public override void OnPickUp()
    {
        if (Inventory.PassiveItemSlot.Add(Item as PassiveItem))
        {
            Item.InitRef_DroppedItem(this);
            Item.Init();
            Item.OnAdd();
            Destroy(gameObject);
        }
    }
}
