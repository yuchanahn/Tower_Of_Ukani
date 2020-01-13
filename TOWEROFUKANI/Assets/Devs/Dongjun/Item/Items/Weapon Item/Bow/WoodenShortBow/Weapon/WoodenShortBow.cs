using UnityEngine;

public class WoodenShortBow : BowController<WoodenShortBowItem>
{
    #region Var: CLA_Action
    private WoodenShortBow_Main_Action action_Main;
    private Bow_Draw_Action action_Draw;
    private Bow_Shoot_Action action_Shoot;
    #endregion

    #region Method: Init
    protected override void Init()
    {
        AddLogic(When.OnDisable, CL_OnDisable);

        AddLogic(ref action_Main, CL_Main);
        AddLogic(ref action_Draw, CL_Draw);
        AddLogic(ref action_Shoot, CL_Shoot);
    }
    #endregion

    #region Method: Condition Logic
    private CLA_Action_Base CL_OnDisable()
    {
        return action_Main;
    }
    private CLA_Action_Base CL_Main()
    {
        if (PlayerWeaponKeys.GetKey(PlayerWeaponKeys.MainAbility))
            return action_Draw;

        return null;
    }
    private CLA_Action_Base CL_Draw()
    {
        if (!action_Draw.IsDrawing)
            return action_Shoot;

        return null;
    }
    private CLA_Action_Base CL_Shoot()
    {
        if (weaponItem.shootTimer.IsEnded)
            return action_Main;

        return null;
    }
    #endregion
}
