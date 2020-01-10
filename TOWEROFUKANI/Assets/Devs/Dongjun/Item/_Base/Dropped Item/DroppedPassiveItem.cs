using UnityEngine;

public class DroppedPassiveItem : DroppedItem
{
    public override void OnPickUp(PlayerItemPickUpData data)
    {
        // TODO!!!

        //// Get Item Reference
        //PassiveItem passiveItem = ItemObj as PassiveItem;

        //// Spawn Item
        //if (!DroppedFromInventory)
        //    passiveItem = Instantiate(passiveItem).GetComponent<PassiveItem>();

        // Add To Inventory
        //if (PassiveInventory.AddExisting(passiveItem))
        //{
        //    Destroy(passiveItem.gameObject);
        //    Destroy(gameObject);
        //    return;
        //}

        //if (PassiveInventory.Add(passiveItem))
        //{
        //    Destroy(gameObject);
        //    return;
        //}
    }
}
