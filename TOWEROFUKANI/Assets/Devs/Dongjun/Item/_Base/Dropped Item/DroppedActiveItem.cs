using UnityEngine;

public class DroppedActiveItem : DroppedItem
{
    public override void OnPickUp()
    {
        ActiveItem activeItem = Item as ActiveItem;

        if (!DroppedFromInventory)
            activeItem = Instantiate(activeItem).GetComponent<ActiveItem>();

        //if (ActiveHotbar.AddExisting(activeItem))
        //{
        //    Destroy(activeItem.gameObject);
        //    Destroy(gameObject);
        //    return;
        //}

        //if (ActiveHotbar.Add(activeItem))
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //if (Inventory.Add(activeItem))
        //{
        //    Destroy(gameObject);
        //    return;
        //}
    }
}
