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
    #endregion

    #region Method: Add/Remove
    public static void Add(int index, ActiveItem item)
    {
        if (Items[index] != null)
            return;

        EmptySlotCount--;
        Items[index] = item;
        item.OnAdd();
    }
    public static void Remove(int index)
    {
        if (Items[index] is null)
            return;

        Items[index].OnRemove();
        Items[index] = null;
        EmptySlotCount++;
    }
    public static void Clear()
    {
        Items = new ActiveItem[SLOT_SIZE];
        EmptySlotCount = SLOT_SIZE;
    }
    #endregion
}
