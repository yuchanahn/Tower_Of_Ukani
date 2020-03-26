using UnityEngine;

public class Overlord : PassiveItem
{
    private int bonusPercentDamage = 10;

    public override void InitStats()
    {
        switch (ItemLevel)
        {
            case 1: break;
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
    protected override void InitStatMod()
    {
        weaponStatMod = weapon =>
        {
            weapon.AttackData.damage.ModPercent += bonusPercentDamage;
        };
    }
}
