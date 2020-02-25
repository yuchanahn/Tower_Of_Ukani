using Dongjun.Helper;
using System;
using UnityEngine;

public abstract class InventoryUIBase : MonoBehaviour
{
    #region Var: Inspector
    [Header("Slot UI")]
    [SerializeField] protected Transform slotUIGroup;
    [SerializeField] protected SlotUI slotUIPrefab;

    [Header("Right Click Action")]
    [SerializeField] protected ItemRightClickAction rightClickAction;
    #endregion

    #region Prop: 
    public InventoryBase inventory
    { get; private set; }
    public int Size
    { get; private set; }

    public SlotUI[] slotUIs
    { get; protected set; }
    public ItemUI[] itemUIs
    { get; protected set; }
    #endregion

    #region Method: Initialize
    public void Init(InventoryBase inventory, Func<int, InventoryBase, int, bool> onMoveItem)
    {
        this.inventory = inventory;
        Size = inventory.Size;

        slotUIs = new SlotUI[Size];
        itemUIs = new ItemUI[Size];

        // Clear Slots
        slotUIGroup.ClearChildren();

        // Spawn Slots
        for (int i = 0; i < Size; i++)
        {
            SlotUI slot = Instantiate(slotUIPrefab.gameObject).GetComponent<SlotUI>();
            slot.transform.SetParent(slotUIGroup, false);
            slot.name = $"Slot_{i}";
            slot.Init(this, i);

            if (slot is DroppableSlotUI)
                (slot as DroppableSlotUI).Init(onMoveItem);

            slotUIs[i] = slot;
        }
    }
    #endregion

    #region Method: Helper
    public bool IsValidIndex(int index)
    {
        return !(index < 0 || index > Size - 1);
    }
    protected abstract ItemUI SpawnItemUI(Item item, int slotIndex);
    #endregion

    #region Method: Inventory UI
    public void Clear()
    {
        for (int i = 0; i < Size; i++)
        {
            if (itemUIs[i] != null)
            {
                Destroy(itemUIs[i].gameObject);
                itemUIs[i] = null;
            }
        }
    }
    public void UpdateSlot(int index)
    {
        Item item = inventory.GetItem(index);

        // Destory if Null
        if (item == null)
        {
            if (itemUIs[index] != null)
            {
                Destroy(itemUIs[index].gameObject);
                itemUIs[index] = null;
            }
            return;
        }
        
        // Spawn UI
        if (itemUIs[index] == null)
            itemUIs[index] = SpawnItemUI(item, index);

        // Update UI
        itemUIs[index].OnUpdateUI(item);
    }
    public void MoveSlot(int from, int to)
    {
        // Swap Ref
        ItemUI buffer = itemUIs[from];
        itemUIs[from] = itemUIs[to];
        itemUIs[to] = buffer;

        // Update UI
        void UpdateUI(int index)
        {
            if (itemUIs[index] == null)
                return;

            itemUIs[index].transform.SetParent(slotUIs[index].transform, false);
            itemUIs[index].OnUpdateUI(inventory.GetItem(index));
        }

        UpdateUI(from);
        UpdateUI(to);
    }
    #endregion
}
