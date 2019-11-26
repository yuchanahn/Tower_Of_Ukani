using UnityEngine;

public class WindCutter : PassiveItem
{
    #region Var: Effects
    private ItemEffect onHitEffect;
    #endregion

    #region Method Override: Add/Remove
    public override void OnAdd()
    {
        onHitEffect = new ItemEffect(GetType(), DebugText);
        ItemEffectManager.AddEffect(PlayerActions.Hit, onHitEffect);
    }
    public override void OnRemove()
    {
        base.OnRemove();

        ItemEffectManager.RemoveEffect(PlayerActions.Hit, onHitEffect);
    }
    #endregion

    #region Method Override: Bonus Stats
    protected override void SetBonusStats(WeaponItem weapon)
    {
        int bonusPercentDamage = 10;

        switch (Count)
        {
            case 1:
                break;
            case 2:
                bonusPercentDamage = 20;
                break;
            default:
                bonusPercentDamage = 100;
                break;
        }

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

    #region Method: Item Effect
    private void DebugText()
    {
        //Debug.Log("Wind Cutter Activated!!");
    }
    #endregion
}
