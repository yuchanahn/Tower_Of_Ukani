using System;
using System.Collections.Generic;
using UnityEngine;

public struct ItemEffect
{
    public int priority;
    public Action action;
}

public class PlayerActionManager : MonoBehaviour
{
    private static List<ItemEffect> onJump = new List<ItemEffect>();
    private static List<ItemEffect> onDash = new List<ItemEffect>();

    private static List<ItemEffect> onDamaged = new List<ItemEffect>();
    private static List<ItemEffect> onHealed = new List<ItemEffect>();

    private static List<ItemEffect> onHit = new List<ItemEffect>();
    private static List<ItemEffect> onKill = new List<ItemEffect>();

    private static List<ItemEffect> onWeaponMainAbility = new List<ItemEffect>();
    private static List<ItemEffect> onWeaponSubAbility = new List<ItemEffect>();
    private static List<ItemEffect> onWeaponSepcialAbility = new List<ItemEffect>();

    private static List<ItemEffect> onGunShoot = new List<ItemEffect>();
    private static List<ItemEffect> onGunReload = new List<ItemEffect>();

    private static List<ItemEffect> onBowShoot = new List<ItemEffect>();


    #region Method: Movement
    public static void OnJump()
    {

    }
    public static void OnDash()
    {

    }
    #endregion

    #region Method: Health Change
    public static void OnDamaged()
    {

    }
    public static void OnHealed()
    {

    }
    #endregion

    #region Method: Combat
    public static void OnHit()
    {

    }
    public static void OnKill()
    {

    }
    #endregion

    #region Method: Weapon
    public static void OnWeaponMainAbility()
    {

    }
    public static void OnWeaponSubAbility()
    {

    }
    public static void OnWeaponSepcialAbility()
    {

    }
    #endregion

    #region Method: Gun
    public static void OnGunShoot()
    {

    }
    public static void OnGunReload()
    {

    }
    #endregion

    #region Method: Bow
    public static void OnBowShoot()
    {

    }
    #endregion
}
