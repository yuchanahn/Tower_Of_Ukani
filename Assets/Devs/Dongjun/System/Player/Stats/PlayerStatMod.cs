using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatMod : MonoBehaviour
{
    private static List<Action> statMod_Player;
    private static List<Action<WeaponItem>> statMod_Weapon;

    private void Awake()
    {
        statMod_Player = new List<Action>();
        statMod_Weapon = new List<Action<WeaponItem>>();
    }

    #region Player Stat Mod
    public static void AddMod_Player(Action mod)
    {
        statMod_Player.Add(mod);
        ApplyMod_Player();
    }
    public static void RemoveMod_Player(Action mod)
    {
        statMod_Player.Remove(mod);
        ApplyMod_Player();
    }
    public static void ApplyMod_Player()
    {
        PlayerStats.Inst.ResetStats();

        // Apply Mod
        foreach (var mod in statMod_Player)
            mod();
    }
    #endregion

    #region Weapon Stat Mod
    public static void AddMod_Weapon(Action<WeaponItem> mod)
    {
        statMod_Weapon.Add(mod);
        ApplyMod_Weapons();
    }
    public static void RemoveMod_Weapon(Action<WeaponItem> mod)
    {
        statMod_Weapon.Remove(mod);
        ApplyMod_Weapons();
    }
    public static void ApplyMod_Weapons()
    {
        // Inventory
        for (int i = 0; i < PlayerInventoryManager.inventory.Size; i++)
        {
            WeaponItem weapon = PlayerInventoryManager.inventory.GetItem(i) as WeaponItem;
            if (weapon == null)
                continue;

            weapon.ResetStats();

            // Apply Mod
            foreach (var mod in statMod_Weapon)
                mod(weapon);
        }

        // Weapon Hotbar
        for (int i = 0; i < PlayerInventoryManager.weaponHotbar.Size; i++)
        {
            WeaponItem weapon = PlayerInventoryManager.weaponHotbar.GetItem(i) as WeaponItem;
            if (weapon == null)
                continue;

            weapon.ResetStats();

            // Apply Mod
            foreach (var mod in statMod_Weapon)
                mod(weapon);
        }
    }
    public static void ApplyMod_Weapon(WeaponItem weapon)
    {
        weapon.ResetStats();

        // Apply Mod
        foreach (var mod in statMod_Weapon)
            mod(weapon);
    }
    #endregion
}
