using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableSlotUI : SlotUI,
    IDropHandler
{
    #region Var: Actions
    private Func<int, InventoryBase, int, bool> onMoveItem;
    #endregion

    #region Var: Slot
    public bool IsLocked = false;
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
        if (IsLocked)
            return;

        DraggableItemUI droppedItem = eventData.pointerDrag.gameObject.GetComponent<DraggableItemUI>();
        if (droppedItem == null)
            return;

        DroppableSlotUI fromSlot = droppedItem.GetComponentInParent<DroppableSlotUI>();
        if (fromSlot == this)
            return;

        if (inventoryUI == null)
        {
            Debug.LogError("Inventory Does Not Exists!!!\nAdd Inventory Component.");
            return;
        }

        // Check IsLocked
        if (inventoryUI.itemUIs[Index]?.Item.LockItemSlot ?? false || droppedItem.Item.LockItemSlot)
            return;

        // Trigger Action
        UI_Screen screen = null;
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
