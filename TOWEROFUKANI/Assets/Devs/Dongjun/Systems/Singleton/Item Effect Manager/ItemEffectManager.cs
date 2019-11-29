using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerActions
{
    // Movement
    Jump,
    Dashing,
    DashStart,
    DashEnd,

    // Health Change
    Damaged,
    Healed,

    // Attack
    Hit,
    Kill,

    // Weapon
    WeaponHit,
    WeaponMain,
    WeaponSub,
    WeaponSpecial,

    // Gun
    GunHit,
    GunShoot,
    GunReload,

    // Bow
    BowHit,
    BowShoot,
}

public class ItemEffectManager : MonoBehaviour
{
    private static Dictionary<PlayerActions, List<ItemEffect>> actions;

    private void Awake()
    {
        if (actions != null)
            return;

        // Init onAction Dictionary
        actions = new Dictionary<PlayerActions, List<ItemEffect>>();

        for (int i = 0; i < Enum.GetValues(typeof(PlayerActions)).Length; i++)
        {
            actions.Add((PlayerActions)i, new List<ItemEffect>());
        }
    }

    public static void Trigger(PlayerActions action)
    {
        for (int i = 0; i < actions[action].Count; i++)
        {
            actions[action][i].ItemAction?.Invoke();
        }
    }
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
}
