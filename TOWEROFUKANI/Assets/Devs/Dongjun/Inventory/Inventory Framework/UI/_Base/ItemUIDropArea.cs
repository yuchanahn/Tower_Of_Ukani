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
        //droppedItem.Item.Inventory.DropItem(droppedItem.Item, droppedItem.Item.Count);
        droppedItem.ItemObj.Inventory.RemoveItem(droppedItem.ItemObj, droppedItem.ItemObj.Info.Count);
    }
}
