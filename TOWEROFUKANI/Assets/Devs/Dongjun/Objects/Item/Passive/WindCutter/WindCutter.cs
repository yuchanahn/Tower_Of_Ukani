using UnityEngine;

public class WindCutter : PassiveItem
{
    #region Var: Inspector
    [SerializeField] private string debugText = "Wind Cutter Activated!!";
    [SerializeField] private float bonusAttackSpeed;
    #endregion

    #region Var: Effects
    protected ItemEffect onJumpEffect = new ItemEffect();
    #endregion

    #region Method: Add/Remove
    public override void OnAdd()
    {
        Max_Count = 3;
        onJumpEffect.action = OnJump;
        ItemEffectManager.AddEffect(PlayerActions.Jump, onJumpEffect);
    }
    #endregion

    #region Method: Bonus Stats
    protected override void SetBonusStats(Item item)
    {
        switch (item)
        {
            case GunItem gun:
                gun.shootTimer.EndTime.ModFlat -= bonusAttackSpeed;
                break;
            default:
                break;
        }
    }
    #endregion

    #region Method: Item Effect
    private void OnJump()
    {
        //Debug.Log(debugText);
    }
    #endregion
}
