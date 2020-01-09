using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableSlotUI : SlotUI,
    IDropHandler
{
    #region Var: Actions
    private Func<int, InventoryBase, int, bool> onMoveItem;
    #endregion

    #region Method: Init
    public virtual void Init(Func<int, InventoryBase, int, bool> onMoveItem)
    {
        this.onMoveItem = onMoveItem;
    }
    #endregion

    #region Method: On Drop
    protected virtual void OnDropSuccess()
    {

    }
    protected virtual void OnDropFail()
    {

    }
    #endregion

    #region Interface: Unity UI Event
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        DraggableItemUI droppedItem = eventData.pointerDrag.gameObject.GetComponent<DraggableItemUI>();
        if (droppedItem == null)
            return;

        DroppableSlotUI fromSlot = droppedItem.GetComponentInParent<DroppableSlotUI>();
        if (fromSlot == this)
            return;

        UI_Screen screen = null;

        // Trigger Action
        if (fromSlot.onMoveItem(fromSlot.Index, inventoryUI.inventory, Index))
        {
            // Drop Success
            OnDropSuccess();

            // Get This Screen
            screen = GetComponentInParent<UI_Screen>();
        }
        else
        {
            // Drop Fail
            OnDropFail();

            // Get Item Screen
            screen = droppedItem.GetComponent<UI_Screen>();
        }

        // Send Screen to Top
        if (screen != null)
            CloseableScreenManager.Inst.SendScreenToTop(screen);
    }
    #endregion
}
