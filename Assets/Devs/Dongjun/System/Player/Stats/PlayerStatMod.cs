using System;
using System.Collections.Generic;
using UnityEngine;

// TODO : 씬에 넣어야 함.
public class PlayerStatMod : MonoBehaviour
{
    private static List<Action> statMod_Player;
    private static List<Action<WeaponItem>> statMod_Weapon;

    private void Awake()
    {
        statMod_Player = new List<Action>();
        statMod_Weapon = new List<Action<WeaponItem>>();
    }

    // Mod Player Stats
    public static void Add_Player(Action mod)
    {
        statMod_Player.Add(mod);
        Apply_Player();
    }
    public static void Remove_Player(Action mod)
    {
        statMod_Player.Remove(mod);
        Apply_Player();
    }

    // Mod Weapon Stats
    public static void Add_Weapon(Action<WeaponItem> mod)
    {
        statMod_Weapon.Add(mod);
        Apply_Weapons();
    }
    public static void Remove_Weapon(Action<WeaponItem> mod)
    {
        statMod_Weapon.Remove(mod);
        Apply_Weapons();
    }

    // Apply Mod
    public static void Apply_Player()
    {
        PlayerStats.Inst.ResetStats();

        for (int i = 0; i < statMod_Player.Count; i++)
            statMod_Player[i]();
    }
    public static void Apply_Weapons()
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
    public static void Apply_Weapon(WeaponItem weapon)
    {
        weapon.ResetStats();

        for (int imod = 0; imod < statMod_Weapon.Count; imod++)
            statMod_Weapon[imod](weapon);
    }
}
