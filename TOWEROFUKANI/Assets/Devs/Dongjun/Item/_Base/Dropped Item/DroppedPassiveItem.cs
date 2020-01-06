using UnityEngine;

public class DroppedPassiveItem : DroppedItem
{
    public override void OnPickUp()
    {
        PassiveItem passiveItem = Item as PassiveItem;

        if (!DroppedFromInventory)
            passiveItem = Instantiate(passiveItem).GetComponent<PassiveItem>();

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
