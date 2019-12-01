using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : SingletonBase<Inventory>
{
    #region Var: Const
    public const int SLOT_SIZE = 15;
    #endregion

    #region Var: Inspector
    //[Serializable] private 
    #endregion

    #region Var: Properties
    public static Item[] Items
    { get; private set; } = new Item[SLOT_SIZE];
    public static int EmptySlotCount
    { get; private set; } = SLOT_SIZE;
    public static bool IsFull => EmptySlotCount == 0;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        Clear();
    }
    #endregion

    #region Method: Add/Remove
    public static bool Add(Item item)
    {
        if (IsFull) return false;

        int index = Array.IndexOf(Items, default);
        if (index != -1)
        {
            Items[index] = item;
            item.OnAdd();
            EmptySlotCount--;
            return true;
        }

        return false;
    }
    public static bool Remove(Item item)
    {
        int index = Array.IndexOf(Items, item);
        if (index != -1)
        {
            Items[index].OnRemove();
            Items[index] = null;
            EmptySlotCount++;
            return true;
        }

        return false;
    }
    public static void Clear()
    {
        Array.Clear(Items, 0, Items.Length);
        EmptySlotCount = SLOT_SIZE;
    }
    #endregion

    #region Reset All Weapon Stats
    public static void ResetAllWeaponStats()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] is null) continue;

            switch (Items[i])
            {
                case WeaponItem weapon:
                    weapon.InitStats();
                    break;
            }
        }

        for (int i = 0; i < WeaponHotbar.SLOT_SIZE; i++)
        {
            WeaponHotbar.Weapons[i]?.InitStats();
        }
    }
    #endregion
}