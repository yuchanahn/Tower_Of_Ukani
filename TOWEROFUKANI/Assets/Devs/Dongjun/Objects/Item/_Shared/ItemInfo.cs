using System.Collections.Generic;
using UnityEngine;

public enum ItemTag_Type
{
    None,
    Physical,
    Magical,
    Living,
}

public enum ItemTag_Range
{
    None,
    Ranged,
    Melee,
}

[System.Serializable]
public sealed class ItemInfo
{
    #region Var: Inspector
    [Header("Item Info")]
    [SerializeField] private Sprite icon;
    [SerializeField] private string name = "Item Name";
    [SerializeField] private string desc = "This is an Item";
    [SerializeField] private ItemTag_Type[] tag_Type;
    [SerializeField] private ItemTag_Range[] tag_Range;
    #endregion

    #region Var: Properties
    public Sprite Icon => icon;
    public string Name => name;
    public string NameTrimed { get; private set; }
    public string Desc => desc;
    public HashSet<ItemTag_Type> Tags_Type { get; private set; } = new HashSet<ItemTag_Type>();
    public HashSet<ItemTag_Range> Tags_Range { get; private set; } = new HashSet<ItemTag_Range>();
    #endregion

    public void Init()
    {
        NameTrimed = Name.Replace(" ", string.Empty);

        if (tag_Type != null)
            for (int i = 0; i < tag_Type.Length; i++)
                Tags_Type.Add(tag_Type[i]);

        if (tag_Range != null)
            for (int i = 0; i < tag_Range.Length; i++)
                Tags_Range.Add(tag_Range[i]);
    }
}
