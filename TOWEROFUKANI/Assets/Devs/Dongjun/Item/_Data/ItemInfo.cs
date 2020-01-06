using System.Collections.Generic;
using UnityEngine;

public enum Item_DamageType
{
    None,
    Physical,
    Magical,
    Living,
}

public enum Item_Range
{
    None,
    Ranged,
    Melee,
}

[System.Serializable]
public sealed class ItemInfo
{
    #region Var: Inspector
    [Header("Data")]
    [SerializeField] private Sprite icon;
    [SerializeField] private string itemName = "Item Name";
    [SerializeField, TextArea] private string itemDesc = "This is an Item";
    [SerializeField] private int itemStackLimit = 0;

    [Header("Item Tags")]
    [SerializeField] private Item_DamageType[] damageType;
    [SerializeField] private Item_Range[] range;
    #endregion

    #region Var: Properties
    // Item Data
    public string ID
    { get; private set; }
    public Sprite Icon => icon;
    public string ItemName => itemName;
    public string NameTrimed { get; private set; }
    public string ItemDesc => itemDesc;

    // Item Count
    private int bk_ItemCount = 1;
    public int Count
    {
        get => bk_ItemCount;
        set { bk_ItemCount = itemStackLimit == 0 ? Mathf.Max(0, value) : Mathf.Clamp(value, 0, itemStackLimit); }
    }
    public int StackLimit
    {
        get => itemStackLimit;
        set { itemStackLimit = Mathf.Max(1, value); }
    }
    public bool IsMaxStack => StackLimit != 0 && Count == StackLimit;

    // Item Tags
    public Item_DamageType[] DamageType => damageType;
    public Item_Range[] Range => range;
    #endregion

    #region Method: Initialize
    public void Init()
    {
        NameTrimed = ItemName.Replace(" ", string.Empty);
        SetID(itemName);
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
