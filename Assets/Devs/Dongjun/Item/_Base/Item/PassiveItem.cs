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
    }
    #endregion

    #region Method: Stats
    // 이 함수로 아이템 스탯 초기화
    public override void InitStats() { }

    // 이 함수에서 playerStatsMod / weaponStatsMod 초기화
    public virtual void InitStatMod() { }
    #endregion

    #region Method: Item
    public override void AddLevel(int amount = 1)
    {
        base.AddLevel(amount);

        if (playerStatMod != null) PlayerStatMod.Apply_Player();
        if (weaponStatMod != null) PlayerStatMod.Apply_Weapons();
    }
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        if (playerStatMod != null) PlayerStatMod.Add_Player(playerStatMod);
        if (weaponStatMod != null) PlayerStatMod.Add_Weapon(weaponStatMod);
    }
    protected override void OnRemovedFromInventory()
    {
        if (playerStatMod != null) PlayerStatMod.Remove_Player(playerStatMod);
        if (weaponStatMod != null) PlayerStatMod.Remove_Weapon(weaponStatMod);
    }
    #endregion
}
