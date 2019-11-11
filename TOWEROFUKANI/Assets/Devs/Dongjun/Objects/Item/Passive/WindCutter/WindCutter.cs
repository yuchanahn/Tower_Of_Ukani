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

    #region Method: Item
    public override void Init()
    {
        base.Init();

        Max_Count = 3;
        onJumpEffect.action = OnJump;
    }

    public override void OnAdd()
    {
        ItemEffectManager.AddEffect(PlayerActions.Jump, onJumpEffect);
    }

    protected override void SetBonusStats(Item item)
    {
        switch (item)
        {
            case GunItem gun:
                gun.shootTimer.EndTime.Mod_Flat -= bonusAttackSpeed;
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
