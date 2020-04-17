using System;
using UnityEngine;

public abstract class PassiveItem : UpgradableItem
{
    #region Var: Inspector
    [SerializeField, Header("God")]
    private TowerOfUkani.Gods god;
    #endregion

    #region Var: Stats Mod
    protected Action playerStatMod = null;
    protected Action<WeaponItem> weaponStatMod = null;
    #endregion

    #region Prop:
    public TowerOfUkani.Gods God => god;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        InitStatMod();
        InitEvents();
    }
    #endregion

    #region Method: Stats
    protected virtual void InitStatMod() { }
    protected virtual void InitEvents() { }
    #endregion

    #region Method: Item
    public override void AddLevel(int amount = 1)
    {
        base.AddLevel(amount);

        if (playerStatMod != null) PlayerStatMod.ApplyMod_Player();
        if (weaponStatMod != null) PlayerStatMod.ApplyMod_Weapons();
    }
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        if (playerStatMod != null) PlayerStatMod.AddMod_Player(playerStatMod);
        if (weaponStatMod != null) PlayerStatMod.AddMod_Weapon(weaponStatMod);
    }
    protected override void OnRemovedFromInventory()
    {
        if (playerStatMod != null) PlayerStatMod.RemoveMod_Player(playerStatMod);
        if (weaponStatMod != null) PlayerStatMod.RemoveMod_Weapon(weaponStatMod);
    }
    #endregion
}
