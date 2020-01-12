using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public sealed class ItemInfo
{
    #region Var: Inspector
    [Header("Data")]
    [SerializeField] private Sprite icon;
    [SerializeField] private string itemName = "Item Name";
    [SerializeField, TextArea] private string itemDesc = "This is an Item";
    [SerializeField] private int itemStackLimit = 1; // 0 이면 무한 스택 가능

    [Header("Item Tags")]
    [SerializeField] private Item_DamageType[] damageType;
    [SerializeField] private Item_Range[] range;
    #endregion

    #region Var: Properties
    // Item Data
    public string ID
    { get; private set; } = null;
    public Sprite Icon => icon;
    public string ItemName => itemName;
    public string ItemDesc => itemDesc;

    // Item Count
    private int bk_Count = 1;
    public int Count
    {
        get => bk_Count;
        set { bk_Count = itemStackLimit == 0 ? Mathf.Max(0, value) : Mathf.Clamp(value, 0, itemStackLimit); }
    }
    public int StackLimit
    {
        get => itemStackLimit;
        set { itemStackLimit = Mathf.Max(1, value); }
    }
    public bool IsMaxStack => StackLimit != 0 && Count == StackLimit;

    // Item Tags
    public List<Item_DamageType> DamageType
    { get; private set; }
    public List<Item_Range> Range
    { get; private set; }
    #endregion

    #region Method: Initialize
    public void Init()
    {
        // Init ID as ItemName
        SetID(itemName);

        // Init Tag
        DamageType = damageType.ToList();
        Range = range.ToList();
    }
    #endregion

    #region Method: Data
    public void SetID(string id)
    {
        ID = id;
    }
    public bool AddCount(ItemInfo other)
    {
        if (ID != other.ID || IsMaxStack)
            return false;

        if (StackLimit == 0)
        {
            Count += other.Count;
            other.Count = 0;
        }
        else
        {
            int countLeft = (StackLimit - Count);
            Count += other.Count;
            other.Count -= countLeft;
        }
        return true;
    }
    #endregion
}
