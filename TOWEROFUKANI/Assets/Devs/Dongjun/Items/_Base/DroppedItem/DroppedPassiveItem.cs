using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedPassiveItem : DroppedItem
{
    public override void OnPickUp()
    {
        if (PassiveInventory.Add(Item as PassiveItem))
        {
            Destroy(gameObject);
        }
    }
}
