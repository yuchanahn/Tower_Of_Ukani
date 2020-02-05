using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffectManager : MonoBehaviour
{
    private static Dictionary<PlayerActions, List<ActionEffect>> effects;

    private void Awake()
    {
        // Init Actions Dictionary
        effects = new Dictionary<PlayerActions, List<ActionEffect>>();

        for (int i = 0; i < Enum.GetValues(typeof(PlayerActions)).Length; i++)
        {
            effects.Add((PlayerActions)i, new List<ActionEffect>());
        }
    }

    public static void Trigger(PlayerActions action)
    {
        for (int i = 0; i < effects[action].Count; i++)
        {
            effects[action][i].EffectAction?.Invoke();
        }
    }
    public static void AddEffect(PlayerActions action, ActionEffect itemEffect)
    {
        if (effects[action].Contains(itemEffect))
            return;

        int index = effects[action].FindIndex(i => itemEffect.ThisType == i.AfterThis);
        if (index != -1)
        {
            effects[action].Insert(index, itemEffect);
            return;
        }

        effects[action].Add(itemEffect);
    }
    public static void RemoveEffect(PlayerActions action, ActionEffect itemEffect)
    {
        if (effects[action].Contains(itemEffect))
            effects[action].Remove(itemEffect);
    }
}
