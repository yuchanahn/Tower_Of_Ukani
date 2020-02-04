using UnityEngine;

public class WoodenShortBow : BowController<WoodenShortBowItem>
{
    #region Var: States
    private WoodenShortBow_Main state_Main;
    private Bow_Draw state_Draw;
    private Bow_Shoot state_Shoot;
    #endregion

    #region Method: Init
    protected override void InitStates()
    {
        SetLogic(ref state_Main, () => 
        {
            if (PlayerWeaponKeys.GetKey(PlayerWeaponKeys.MainAbility))
                return state_Draw;

            return null;
        });

        SetLogic(ref state_Draw, () =>
        {
            if (!state_Draw.IsDrawing)
                return state_Shoot;

            return null;
        });

        SetLogic(ref state_Shoot, () =>
        {
            if (weaponItem.shootTimer.IsEnded)
                return state_Main;

            return null;
        });

        SetLogic(When.OnDisable, () => state_Main);

        SetDefaultState(state_Main);
    }
    #endregion
}
