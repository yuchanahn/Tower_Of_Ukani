using UnityEngine;

public class Overlord : PassiveItem
{
    private int bonusPercentDamage = 10;

    #region Method: Initialize
    public override void InitStats()
    {
        switch (ItemLevel)
        {
            case 1:
                break;
            case 2:
                bonusPercentDamage = 50;
                break;
            case 3:
                bonusPercentDamage = 100;
                break;
            default:
                bonusPercentDamage = 200;
                break;
        }
    }
    #endregion

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);
    }

    #region Method Override: Bonus Stats
    public override void ApplyBonusStats(WeaponItem weapon)
    {
        switch (weapon)
        {
            case GunItem gun:
                gun.attackData.damage.ModPercent += bonusPercentDamage;
                break;

            case BowItem bow:
                bow.attackData.damage.ModPercent += bonusPercentDamage;
                break;
        }
    }
    #endregion
}
