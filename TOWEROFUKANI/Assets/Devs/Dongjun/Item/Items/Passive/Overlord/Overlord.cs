using UnityEngine;

public class Overlord : PassiveItem
{
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

    }

    #region Method Override: Bonus Stats
    protected override void SetBonusStats(WeaponItem weapon)
    {
        int bonusPercentDamage = 10;

        //switch (Count)
        //{
        //    case 1:
        //        break;
        //    case 2:
        //        bonusPercentDamage = 50;
        //        break;
        //    case 3:
        //        bonusPercentDamage = 100;
        //        break;
        //    default:
        //        bonusPercentDamage = 200;
        //        break;
        //}

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
