using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActiveHotbar : SingletonBase<ActiveHotbar>
{
    #region Var: Const
    public const int SLOT_SIZE = 4;
    #endregion

    #region Var: Inspector
    [Header("Sprite")]
    [SerializeField] private Sprite spr_Empty;

    [Header("Active Slot")]
    [SerializeField] private Image[] img_ActiveSlots = new Image[SLOT_SIZE];
    #endregion

    #region Var: Properties
    public static ActiveItem[] Items { get; private set; } = new ActiveItem[4];
    public static int EmptySlotCount { get; private set; } = Items.Length;
    public static bool IsFull => EmptySlotCount == 0;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        Clear();
        // Init UI
    }
    private void Update()
    {
        ActivateItem();
    }
    #endregion

    #region Method: UI
    private void UpdateUI_SlotIcon()
    {
        for (int i = 0; i < SLOT_SIZE; i++)
        {
            img_ActiveSlots[i].sprite = Items[i]?.Info.Icon ?? spr_Empty;
        }
    }
    #endregion

    #region Method: Activate Item
    private void ActivateItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && (!Items[0]?.IsActive ?? false))
            Items[0]?.Activate();
        else if (Input.GetKeyDown(KeyCode.Alpha2) && (!Items[1]?.IsActive ?? false))
            Items[1]?.Activate();
        else if (Input.GetKeyDown(KeyCode.Alpha3) && (!Items[2]?.IsActive ?? false))
            Items[2]?.Activate();
        else if (Input.GetKeyDown(KeyCode.Alpha4) && (!Items[3]?.IsActive ?? false))
            Items[3]?.Activate();
    }
    #endregion

    #region Method: Add/Remove
    public static bool AddExisting(ActiveItem item)
    {
        ActiveItem existingItem = Items.FirstOrDefault(e => e != null && e.Info.Name == item.Info.Name);

        if (existingItem == null)
            existingItem = Inventory.Items.FirstOrDefault(e => e != null && e.Info.Name == item.Info.Name) as ActiveItem;

        if (existingItem == null)
            return false;

        existingItem.AddCount(item.Count);
        return true;
    }
    public static bool Add(ActiveItem item)
    {
        if (IsFull)
            return false;

        int index = Array.IndexOf(Items, default);
        if (index == -1)
            return false;

        item.OnAdd();
        Items[index] = item;
        EmptySlotCount--;

        Inst.UpdateUI_SlotIcon();
        return true;
    }
    public static void Remove(int index)
    {
        if (Items[index] is null)
            return;

        Items[index].OnRemove();
        Items[index] = null;
        EmptySlotCount++;

        Inst.UpdateUI_SlotIcon();
    }
    public static void Clear()
    {
        Items = new ActiveItem[SLOT_SIZE];
        EmptySlotCount = SLOT_SIZE;
    }
    #endregion
}
