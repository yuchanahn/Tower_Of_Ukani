using UnityEngine;

public class WindCutter : PassiveItem
{
    #region Var: Effects
    private ItemEffect onHitEffect;
    private int bonusDamage = 1;
    #endregion

    #region Method Override: Add/Remove
    public override void OnAdd()
    {
        base.OnAdd();

        onHitEffect = new ItemEffect(typeof(WindCutter), DebugText);
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
        switch (weapon)
        {
            case GunItem gun:
                gun.bulletData.attackData.damage.ModFlat += bonusDamage;
                break;

            case BowItem bow:
                bow.arrowData.attackData.damage.ModFlat += bonusDamage;
                break;
        }
    }
    #endregion

    #region Method: Item Effect
    private void DebugText()
    {
        Debug.Log("Wind Cutter Activated!!");
    }
    #endregion
}
