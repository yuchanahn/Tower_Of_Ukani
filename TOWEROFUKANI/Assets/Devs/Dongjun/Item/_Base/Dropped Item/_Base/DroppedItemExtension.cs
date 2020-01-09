using UnityEngine;

public static class DroppedItemExtension
{
    public static DroppedItem SpawnDroppedItem(this Item item)
    {
        DroppedItem droppedItem =  GameObject.Instantiate(item.DroppedItemPrefab, item.transform.position, Quaternion.identity).GetComponent<DroppedItem>();
        droppedItem.SetRef_Item(item);
        return droppedItem;
    }
}
