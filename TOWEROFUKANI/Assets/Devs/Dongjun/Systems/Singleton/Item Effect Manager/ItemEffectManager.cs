using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerActions
{
    Jump,
    Dash,

    Damaged,
    Healed,
    Hit,
    Kill,

    WeaponMain,
    WeaponSub,
    WeaponSpecial,

    GunShoot,
    GunReload,

    BowShoot,

    END
}

public class ItemEffect
{
    public Type ThisType { get; private set; }
    public Action ItemAction { get; private set; }
    public Type After { get; private set; }

    public ItemEffect(Type thisType, Action action = null, Type after = null)
    {
        ThisType = thisType;
        ItemAction = action;
        After = after;
    }
}

public class ItemEffectManager : MonoBehaviour
{
    private static Dictionary<PlayerActions, List<ItemEffect>> onAction;

    private void Awake()
    {
        if (onAction != null)
            return;

        // Reset Dictionary
        onAction = new Dictionary<PlayerActions, List<ItemEffect>>();

        // Init onAction Dictionary
        for (int i = 0; i < (int)PlayerActions.END; i++)
            onAction.Add((PlayerActions)i, new List<ItemEffect>());
    }

    public static void Trigger(PlayerActions action)
    {
        for (int i = 0; i < onAction[action].Count; i++)
        {
            onAction[action][i].ItemAction?.Invoke();
        }
    }
    public static void AddEffect(PlayerActions action, ItemEffect itemEffect)
    {
        if (!onAction[action].Contains(itemEffect))
        {
            for (int i = 0; i < onAction[action].Count; i++)
            {
                if (itemEffect.ThisType == onAction[action][i].After)
                {
                    onAction[action].Insert(i, itemEffect);
                    return;
                }
            }

            onAction[action].Add(itemEffect);
        }
    }
    public static void RemoveEffect(PlayerActions action, ItemEffect itemEffect)
    {
        if (onAction[action].Contains(itemEffect))
            onAction[action].Remove(itemEffect);
    }
}
