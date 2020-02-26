using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUIDropArea : MonoBehaviour,
        IDropHandler
{
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        DraggableItemUI droppedItem = eventData.pointerDrag.gameObject.GetComponent<DraggableItemUI>();
        if (droppedItem == null)
            return;

        // Drop Item
        InventoryBase inventory = droppedItem.Item.Inventory;
        inventory.DropItem(inventory.GetIndex_ItemID(droppedItem.Item.Info.ID));
    }
}
