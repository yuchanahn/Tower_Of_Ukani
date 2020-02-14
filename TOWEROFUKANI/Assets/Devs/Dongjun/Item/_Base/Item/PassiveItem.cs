using System;
using UnityEngine;

public abstract class PassiveItem : UpgradableItem
{
    // Inspector
    [SerializeField, Header("God")]
    private TowerOfUkani.Gods god;

    // Stats Mod
    protected Action playerStatsMod = null;
    protected Action<WeaponItem> weaponStatsMod = null;

    // Properties
    public TowerOfUkani.Gods God => god;

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

        if (playerStatsMod != null) PlayerStatMod.Apply_Player();
        if (weaponStatsMod != null) PlayerStatMod.Apply_Weapons();
    }
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        if (playerStatsMod != null) PlayerStatMod.Add_Player(playerStatsMod);
        if (weaponStatsMod != null) PlayerStatMod.Add_Weapon(weaponStatsMod);
    }
    public override void OnDrop()
    {
        base.OnDrop();

        if (playerStatsMod != null) PlayerStatMod.Remove_Player(playerStatsMod);
        if (weaponStatsMod != null) PlayerStatMod.Remove_Weapon(weaponStatsMod);
    }
    #endregion
}
