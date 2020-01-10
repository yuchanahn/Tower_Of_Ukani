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
        for (int i = 1; i < Enum.GetValues(typeof(TowerOfUkani.Gods)).Length; i++)
        {
            // Load 할때 저장된 맥스 사이즈 가져오기
            divineMaxSizes.Add((TowerOfUkani.Gods)i, 3);

            // Init Divine Relic Inventory
            divineRelics.Add((TowerOfUkani.Gods)i, new List<PassiveItem>());
        }
    }
    #endregion

    #region Method: Helper
    public bool Contains(PassiveItem item, TowerOfUkani.Gods god = TowerOfUkani.Gods.None)
    {
        switch (god)
        {
            case TowerOfUkani.Gods.None:
                return relics.Find(i => i.Info.ItemName == item.Info.ItemName) != null;
            default:
                return divineRelics[god].Find(i => i.Info.ItemName == item.Info.ItemName) != null;
        }
    }
    #endregion

    #region Method: Inventory
    public bool TryAddItem(PassiveItem item, TowerOfUkani.Gods god = TowerOfUkani.Gods.None)
    {
        if (Contains(item, god))
            return false;

        switch (god)
        {
            case TowerOfUkani.Gods.None:
                relics.Add(item);
                return true;
            default:
                if (divineRelics[god].Count >= divineMaxSizes[god]) return false;
                divineRelics[god].Add(item);
                return true;
        }
    }
    public bool TryRemoveItem(PassiveItem item, TowerOfUkani.Gods god = TowerOfUkani.Gods.None)
    {
        if (!Contains(item, god))
            return false;

        switch (god)
        {
            case TowerOfUkani.Gods.None:
                relics.RemoveAt(relics.FindIndex(i => i.Info.ItemName == item.Info.ItemName));
                return true;
            default:
                divineRelics[god].RemoveAt(divineRelics[god].FindIndex(i => i.Info.ItemName == item.Info.ItemName));
                return true;
        }
    }
    #endregion

    #region Method: Bonus Stats
    public void ApplyBonusStats(ToU_Inventory inventory)
    {

    }
    #endregion
}
