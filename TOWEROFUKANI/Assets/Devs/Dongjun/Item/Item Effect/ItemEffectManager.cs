using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectManager : MonoBehaviour
{
    private static Dictionary<PlayerActions, List<ItemEffect>> actions;

    #region Method: Unity
    private void Awake()
    {
        // Init Actions Dictionary
        actions = new Dictionary<PlayerActions, List<ItemEffect>>();

        for (int i = 0; i < Enum.GetValues(typeof(PlayerActions)).Length; i++)
        {
            actions.Add((PlayerActions)i, new List<ItemEffect>());
        }
    }
    #endregion

    #region Method: Trigger Effect
    public static void Trigger(PlayerActions action)
    {
        for (int i = 0; i < actions[action].Count; i++)
        {
            actions[action][i].ItemAction?.Invoke();
        }
    }
    #endregion

    #region Method: Manage Effects
    public static void AddEffect(PlayerActions action, ItemEffect itemEffect)
    {
        if (actions[action].Contains(itemEffect))
            return;

        for (int i = 0; i < actions[action].Count; i++)
        {
            if (itemEffect.ThisType == actions[action][i].After)
            {
                actions[action].Insert(i, itemEffect);
                return;
            }
        }

        actions[action].Add(itemEffect);
    }
    public static void RemoveEffect(PlayerActions action, ItemEffect itemEffect)
    {
        if (actions[action].Contains(itemEffect))
            actions[action].Remove(itemEffect);
    }
    #endregion
}
