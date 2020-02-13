using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStatMod
{
    private static List<Action> statMod_Player = new List<Action>();
    private static List<Action<WeaponItem>> statMod_Weapon = new List<Action<WeaponItem>>();

    // Mod Player Stats
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

    // Mod Weapon Stats
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

    // Apply Mod
    public static void ApplyMod_Player()
    {
        PlayerStats.Inst.ResetStats();

        for (int i = 0; i < statMod_Player.Count; i++)
            statMod_Player[i]();
    }
    public static void ApplyMod_Weapons()
    {
        // Inventory
        for (int i = 0; i < PlayerInventoryManager.inventory.Size; i++)
        {
            WeaponItem weapon = PlayerInventoryManager.inventory.GetItem(i) as WeaponItem;
            if (weapon == null) continue;

            weapon.ResetStats();

            for (int imod = 0; imod < statMod_Weapon.Count; imod++)
                statMod_Weapon[imod](weapon);
        }

        // Weapon Hotbar
        for (int i = 0; i < PlayerInventoryManager.weaponHotbar.Size; i++)
        {
            WeaponItem weapon = PlayerInventoryManager.weaponHotbar.GetItem(i) as WeaponItem;
            if (weapon == null) continue;

            weapon.ResetStats();

            for (int imod = 0; imod < statMod_Weapon.Count; imod++)
                statMod_Weapon[imod](weapon);
        }
    }
    public static void ApplyMod_Weapon(WeaponItem weapon)
    {
        weapon.ResetStats();

        for (int imod = 0; imod < statMod_Weapon.Count; imod++)
            statMod_Weapon[imod](weapon);
    }
}
