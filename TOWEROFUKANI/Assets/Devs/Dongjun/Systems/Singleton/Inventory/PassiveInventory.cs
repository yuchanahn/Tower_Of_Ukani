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
    public static Dictionary<Type, PassiveItem> NormalRelics 
    { get; private set; } = new Dictionary<Type, PassiveItem>();
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        Clear();
    }
    #endregion

    #region Method: Add/Remove
    public static bool AddExisting(PassiveItem item)
    {
        PassiveItem existingItem = null;

        switch (item.God)
        {
            case TowerOfUkani.Gods.None:
                NormalRelics.TryGetValue(item.GetType(), out existingItem);
                break;

            case TowerOfUkani.Gods.Ukani:
                existingItem = ukaniRelics.FirstOrDefault(e => e != null && e.Info.Name == item.Info.Name);
                break;

            case TowerOfUkani.Gods.Bazik:
                existingItem = bazikRelics.FirstOrDefault(e => e != null && e.Info.Name == item.Info.Name);
                break;

            case TowerOfUkani.Gods.Ellai:
                existingItem = ellaiRelics.FirstOrDefault(e => e != null && e.Info.Name == item.Info.Name);
                break;
        }

        if (existingItem == null)
            return false;

        existingItem.AddCount(item.Count);
        ApplyAllBonusStats();
        return true;
    }
    public static bool Add(PassiveItem item)
    {
        bool AddDivineRelic(ref int emptySlotCount, PassiveItem[] items)
        {
            if (emptySlotCount == 0)
                return false;

            items.Push(item);
            item.OnAdd();
            emptySlotCount--;
            ApplyAllBonusStats();
            return true;
        }
        void AddNormalRelic()
        {
            NormalRelics.Add(item.GetType(), item);
            item.OnAdd();
            ApplyAllBonusStats();
        }

        switch (item.God)
        {
            case TowerOfUkani.Gods.None:
                AddNormalRelic();
                return true;

            case TowerOfUkani.Gods.Ukani:
                return AddDivineRelic(ref emptySlotCount_Ukani, ukaniRelics);

            case TowerOfUkani.Gods.Bazik:
                return AddDivineRelic(ref emptySlotCount_Bazik, bazikRelics);

            case TowerOfUkani.Gods.Ellai:
                return AddDivineRelic(ref emptySlotCount_Ellai, ellaiRelics);
        }

        return false;
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
        Array.Clear(ukaniRelics, 0, ukaniRelics.Length);
        Array.Clear(bazikRelics, 0, bazikRelics.Length);
        Array.Clear(ellaiRelics, 0, ellaiRelics.Length);
        NormalRelics.Clear();
    }
    #endregion

    #region Method: Apply All Bonus Stats
    public static void ApplyAllBonusStats()
    {
        // Reset All Weapon Stats
        Inventory.ResetAllWeaponStats();

        // Apply Bonus Stats
        for (int i = 0; i < SLOT_SIZE_PER_GOD; i++)
        {
            UkaniRelics[i]?.ApplyBonusStats();
            BazikRelics[i]?.ApplyBonusStats();
            EllaiRelics[i]?.ApplyBonusStats();
        }
        foreach (KeyValuePair<Type, PassiveItem> item in NormalRelics)
        {
            item.Value.ApplyBonusStats();
        }
    }
    #endregion
}
