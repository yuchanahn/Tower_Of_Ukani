using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectManager : MonoBehaviour
{
    private static Dictionary<PlayerActions, List<ItemEffect>> effects;

    private void Awake()
    {
        // Init Actions Dictionary
        effects = new Dictionary<PlayerActions, List<ItemEffect>>();

        for (int i = 0; i < Enum.GetValues(typeof(PlayerActions)).Length; i++)
        {
            effects.Add((PlayerActions)i, new List<ItemEffect>());
        }
    }

    public static void Trigger(PlayerActions action)
    {
        for (int i = 0; i < effects[action].Count; i++)
        {
            effects[action][i].ItemAction?.Invoke();
        }
    }
    public static void AddEffect(PlayerActions action, ItemEffect itemEffect)
    {
        if (effects[action].Contains(itemEffect))
            return;

        for (int i = 0; i < effects[action].Count; i++)
        {
            if (itemEffect.ThisType == effects[action][i].AfterThis)
            {
                effects[action].Insert(i, itemEffect);
                return;
            }
        }

        effects[action].Add(itemEffect);
    }
    public static void RemoveEffect(PlayerActions action, ItemEffect itemEffect)
    {
        if (effects[action].Contains(itemEffect))
            effects[action].Remove(itemEffect);
    }
}
