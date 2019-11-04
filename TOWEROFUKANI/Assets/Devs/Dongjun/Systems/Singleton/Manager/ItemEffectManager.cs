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
    public ItemEffect after = null;
    public Action action = null;
}

public class ItemEffectManager : MonoBehaviour
{
    private static Dictionary<PlayerActions, List<ItemEffect>> onAction = null;

    private void Awake()
    {
        if (onAction != null)
            return;

        onAction = new Dictionary<PlayerActions, List<ItemEffect>>();

        // Init onAction Dictionary
        for (int i = 0; i < (int)PlayerActions.END; i++)
            onAction.Add((PlayerActions)i, new List<ItemEffect>());
    }

    public static void Trigger(PlayerActions action)
    {
        for (int i = 0; i < onAction[action].Count; i++)
        {
            onAction[action][i].action?.Invoke();
        }
    }

    public static void AddEffect(PlayerActions action, ItemEffect itemEffect)
    {
        // 같은 아이템이 이미 있을 경우
        if (onAction[action].Contains(itemEffect))
        {

        }
        // 같은 아이템이 없을 경우
        else
        {
            if (itemEffect.after is null)
            {
                onAction[action].Add(itemEffect);
                return;
            }

            int index = onAction[action].IndexOf(itemEffect.after);

            if (index != -1)
            {
                onAction[action].Insert(index + 1, itemEffect);
            }
            else
            {
                onAction[action].Add(itemEffect);
            }
        }
    }
    public static void RemoveEffect(PlayerActions action, ItemEffect itemEffect)
    {
        if (onAction[action].Contains(itemEffect))
            onAction[action].Remove(itemEffect);
    }
}
