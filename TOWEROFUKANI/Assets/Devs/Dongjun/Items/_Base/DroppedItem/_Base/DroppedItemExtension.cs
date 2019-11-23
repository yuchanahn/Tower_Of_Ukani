using UnityEngine;

public static class DroppedItemExtension
{
    public static void SpawnDroppedItem(this Item item)
    {
        GameObject.Instantiate(item.DroppedItemPrefab, item.transform.position, Quaternion.identity).
            GetComponent<DroppedItem>().SetRef_Item(item);
    }
}
