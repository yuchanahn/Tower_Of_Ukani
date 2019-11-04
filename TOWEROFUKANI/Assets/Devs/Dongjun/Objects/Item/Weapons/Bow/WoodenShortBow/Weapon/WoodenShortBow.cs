using UnityEngine;

public class WoodenShortBow : BowController<WoodenShotBowItem>
{
    #region Var: CLA_Action
    private WoodenShortBow_Main_Action main_AC;
    private Bow_Draw_Action draw_AC;
    #endregion

    #region Method: Init
    protected override void Init()
    {
        main_AC = GetComponent<WoodenShortBow_Main_Action>();
        draw_AC = GetComponent<Bow_Draw_Action>();

        ConditionLogics.Add(main_AC, CL_Main);
        ConditionLogics.Add(draw_AC, CL_Draw);
    }
    #endregion

    #region Method: Condition Logic
    private CLA_Action CL_Main()
    {
        if (!weaponItem.IsSelected)
            return DefaultAction;

        if (weaponItem.shootTimer.IsEnded && Input.GetKey(PlayerWeaponKeys.MainAbility))
            return draw_AC;

        return main_AC;
    }
    private CLA_Action CL_Draw()
    {
        if (!weaponItem.IsSelected)
            return DefaultAction;

        if (!draw_AC.IsDrawing)
            return main_AC;

        return draw_AC;
    }
    #endregion
}
