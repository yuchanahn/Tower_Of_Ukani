using System;
using UnityEngine;

public abstract class PassiveItem : UpgradableItem
{
    // Inspector
    [SerializeField, Header("God")]
    private TowerOfUkani.Gods god;

    // Stats Mod
    private Action playerStatsMod = null;
    private Action<WeaponItem> weaponStatsMod = null;

    // Properties
    public TowerOfUkani.Gods God => god;

    #region Method: Stats
    // 이 함수로 아이템 스탯 초기화
    public override void InitStats() { }

    // 이 함수에서 AddPlayerStatMod(), AddWeaponStatMod() 사용
    public virtual void InitStatMod() { }

    // 이 함수를 이용하여 플레이어 스탯 변경
    protected void AddPlayerStatMod(Action playerStatsMod)
    {
        this.playerStatsMod = playerStatsMod;
    }

    // 이 함수를 이용하여 무기 스탯 변경
    protected void AddWeaponStatMod(Action<WeaponItem> weaponStatsMod)
    {
        this.weaponStatsMod = weaponStatsMod;
    }
    #endregion

    #region Method: Item
    public override void AddLevel(int amount = 1)
    {
        base.AddLevel(amount);

        if (playerStatsMod != null) PlayerStatMod.ApplyMod_Player();
        if (weaponStatsMod != null) PlayerStatMod.ApplyMod_Weapons();
    }
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        if (playerStatsMod != null) PlayerStatMod.AddMod_Player(playerStatsMod);
        if (weaponStatsMod != null) PlayerStatMod.AddMod_Weapon(weaponStatsMod);
    }
    public override void OnDrop()
    {
        base.OnDrop();

        if (playerStatsMod != null) PlayerStatMod.RemoveMod_Player(playerStatsMod);
        if (weaponStatsMod != null) PlayerStatMod.RemoveMod_Weapon(weaponStatsMod);
    }
    #endregion
}
