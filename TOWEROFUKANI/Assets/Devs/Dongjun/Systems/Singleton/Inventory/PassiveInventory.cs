using Dongjun.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassiveInventory : SingletonBase<PassiveInventory>
{
    #region Var: Const
    public const int SLOT_SIZE_PER_GOD = 3;
    #endregion

    #region Var: Inventory
    private static PassiveItem[] ukaniRelics = new PassiveItem[SLOT_SIZE_PER_GOD];
    private static PassiveItem[] bazikRelics = new PassiveItem[SLOT_SIZE_PER_GOD];
    private static PassiveItem[] ellaiRelics = new PassiveItem[SLOT_SIZE_PER_GOD];

    private static int emptySlotCount_Ukani = SLOT_SIZE_PER_GOD;
    private static int emptySlotCount_Bazik = SLOT_SIZE_PER_GOD;
    private static int emptySlotCount_Ellai = SLOT_SIZE_PER_GOD;
    #endregion

    #region Var: Properties
    public static PassiveItem[] UkaniRelics => ukaniRelics;
    public static PassiveItem[] BazikRelics => bazikRelics;
    public static PassiveItem[] EllaiRelics => ellaiRelics;
    public static Dictionary<Type, PassiveItem> Relics 
    { get; private set; } = new Dictionary<Type, PassiveItem>();
    #endregion

    #region Method: Add/Remove
    public static bool Add(PassiveItem item)
    {
        bool AddItem(ref int emptySlotCount, PassiveItem[] items)
        {
            PassiveItem sameItem = items.FirstOrDefault(s => s != null && s.GetType() == item.GetType());
            if (sameItem != null)
            {
                sameItem.AddCount(item.Count);
                return true;
            }

            if (emptySlotCount == 0)
                return false;

            item.OnAdd();
            emptySlotCount--;
            items.Push(item);
            ApplyAllBonusStats();
            return true;
        }

        switch (item.God)
        {
            case TowerOfUkani.Gods.Ukani:
                return AddItem(ref emptySlotCount_Ukani, ukaniRelics);

            case TowerOfUkani.Gods.Bazik:
                return AddItem(ref emptySlotCount_Bazik, bazikRelics);

            case TowerOfUkani.Gods.Ellai:
                return AddItem(ref emptySlotCount_Ellai, ellaiRelics);

            case TowerOfUkani.Gods.None:
                if (Relics.ContainsKey(item.GetType()))
                {
                    Relics[item.GetType()].AddCount(1);
                    return true;
                }
                item.OnAdd();
                Relics.Add(item.GetType(), item);
                ApplyAllBonusStats();
                return true;

            default:
                return false;
        }
    }
    public static void Remove(PassiveItem item)
    {
        int index;
        void RemoveItem(ref int emptySlotCount, ref PassiveItem[] items)
        {
            index = Array.IndexOf(items, item);
            if (index != -1)
            {
                item.OnRemove();
                items[index] = null;
                emptySlotCount++;
                ApplyAllBonusStats();
            }
        }

        switch (item.God)
        {
            case TowerOfUkani.Gods.Ukani:
                RemoveItem(ref emptySlotCount_Ukani, ref ukaniRelics);
                break;

            case TowerOfUkani.Gods.Bazik:
                RemoveItem(ref emptySlotCount_Bazik, ref bazikRelics);
                break;

            case TowerOfUkani.Gods.Ellai:
                RemoveItem(ref emptySlotCount_Ellai, ref ellaiRelics);
                break;
        }
    }
    public static void Clear()
    {
        ukaniRelics = new PassiveItem[SLOT_SIZE_PER_GOD];
        bazikRelics = new PassiveItem[SLOT_SIZE_PER_GOD];
        ellaiRelics = new PassiveItem[SLOT_SIZE_PER_GOD];
        Relics = new Dictionary<Type, PassiveItem>();
    }
    #endregion

    #region Method: Apply All Bonus Stats
    public static void ApplyAllBonusStats()
    {
        // Reset All Weapon Stats
        Inventory.ResetAllWeaponStats();

        // Apply Bonus Stats
        foreach (KeyValuePair<Type, PassiveItem> item in Relics)
        {
            item.Value.ApplyBonusStats();
        }

        for (int i = 0; i < SLOT_SIZE_PER_GOD; i++)
        {
            UkaniRelics[i]?.ApplyBonusStats();
            BazikRelics[i]?.ApplyBonusStats();
            EllaiRelics[i]?.ApplyBonusStats();
        }
    }
    #endregion
}
