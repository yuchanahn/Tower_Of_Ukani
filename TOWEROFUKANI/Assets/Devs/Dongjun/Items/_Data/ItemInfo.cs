using System.Collections.Generic;
using UnityEngine;

public enum ItemTag_DamageType
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
    [SerializeField] private Sprite icon;
    [SerializeField] private string name = "Item Name";
    [SerializeField] private string desc = "This is an Item";
    [SerializeField] private ItemTag_DamageType[] tag_DamageType;
    [SerializeField] private ItemTag_Range[] tag_Range;
    #endregion

    #region Var: Properties
    public Sprite Icon => icon;
    public string Name => name;
    public string NameTrimed { get; private set; }
    public string Desc => desc;
    public ItemTag_DamageType[] Tag_DamageType => tag_DamageType;
    public ItemTag_Range[] Tag_Range => tag_Range;
    #endregion

    public void Init()
    {
        NameTrimed = Name.Replace(" ", string.Empty);
    }
}
