using Dongjun.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveInventory : MonoBehaviour
{
    #region Var: Inventory
    private Dictionary<TowerOfUkani.Gods, List<PassiveItem>> divineRelics = new Dictionary<TowerOfUkani.Gods, List<PassiveItem>>();
    private Dictionary<TowerOfUkani.Gods, int> divineMaxSizes = new Dictionary<TowerOfUkani.Gods, int>();
    private List<PassiveItem> relics = new List<PassiveItem>();
    #endregion

    #region Method: Unity
    private void Awake()
    {
        // None이 첫번째라서 1부터 시작 
        for (int i = 1; i < General.EnumCount<TowerOfUkani.Gods>(); i++)
        {
            // Load 할때 저장된 맥스 사이즈 가져오기
            divineMaxSizes.Add((TowerOfUkani.Gods)i, 3);

            // Init Divine Relic Inventory
            divineRelics.Add((TowerOfUkani.Gods)i, new List<PassiveItem>());
        }
    }
    #endregion

    #region Method: Helper
    public bool Contains(PassiveItem item)
    {
        switch (item.God)
        {
            case TowerOfUkani.Gods.None:
                return relics.Find(i => i.Info.ItemName == item.Info.ItemName) != null;
            default:
                return divineRelics[item.God].Find(i => i.Info.ItemName == item.Info.ItemName) != null;
        }
    }
    #endregion

    #region Method: Inventory
    public bool TryUpgradeItem(PassiveItem item, params ToU_Inventory[] inventories)
    {
        if (!Contains(item))
            return false;

        PassiveItem upgradeTarget;

        switch (item.God)
        {
            case TowerOfUkani.Gods.None:
                upgradeTarget = relics.Find(i => i.Info.ItemName == item.Info.ItemName);
                break;
            default:
                upgradeTarget = divineRelics[item.God].Find(i => i.Info.ItemName == item.Info.ItemName);
                break;
        }

        upgradeTarget.AddLevel();

        ApplyBonusStatsToInventories(upgradeTarget, inventories);

        return true;
    }
    public bool TryAddItem(PassiveItem item, params ToU_Inventory[] inventories)
    {
        if (Contains(item))
            return false;

        item.OnAdd(null);

        switch (item.God)
        {
            case TowerOfUkani.Gods.None:
                relics.Add(item);
                break;
            default:
                if (divineRelics[item.God].Count >= divineMaxSizes[item.God]) return false;
                divineRelics[item.God].Add(item);
                break;
        }

        ApplyBonusStatsToInventories(item, inventories);

        return true;
    }
    public bool TryRemoveItem(PassiveItem item)
    {
        if (!Contains(item))
            return false;

        PassiveItem passive;

        switch (item.God)
        {
            case TowerOfUkani.Gods.None:
                passive = relics[relics.FindIndex(i => i.Info.ItemName == item.Info.ItemName)];
                relics.Remove(passive);
                break;
            default:
                passive = divineRelics[item.God][divineRelics[item.God].FindIndex(i => i.Info.ItemName == item.Info.ItemName)];
                divineRelics[item.God].Remove(passive);
                break;
        }

        passive.OnRemove();
        return true;
    }
    #endregion

    #region Method: Bonus Stats
    private void ApplyBonusStatsToInventories(PassiveItem item, ToU_Inventory[] inventories)
    {
        // Apply Bonus Stats
        for (int i = 0; i < inventories.Length; i++)
        {
            for (int j = 0; j < inventories[i].Size; j++)
            {
                if (!(inventories[i].GetItem(j) is WeaponItem))
                    continue;

                item.ApplyBonusStats(inventories[i].GetItem(j) as WeaponItem);
            }
        }
    }
    public void ApplyBonusStatsToWeapon(WeaponItem weapon)
    {
        for (int i = 0; i < relics.Count; i++)
        {
            relics[i].ApplyBonusStats(weapon);
        }

        for (int i = 1; i < General.EnumCount<TowerOfUkani.Gods>(); i++)
        {
            for (int j = 0; j < divineRelics[(TowerOfUkani.Gods)i].Count; j++)
            {
                divineRelics[(TowerOfUkani.Gods)i][j].ApplyBonusStats(weapon);
            }
        }
    }
    #endregion
}
