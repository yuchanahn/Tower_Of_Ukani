using UnityEngine;

public class WoodenShortBow : Bow
{
    private WoodenShortBow_Main_Action main_AC;
    private Bow_Draw_Action draw_AC;

    protected override void Init()
    {
        main_AC = GetComponent<WoodenShortBow_Main_Action>();
        draw_AC = GetComponent<Bow_Draw_Action>();

        ConditionLogics.Add(main_AC, CL_Main);
        ConditionLogics.Add(draw_AC, CL_Draw);
    }

    private CLA_Action CL_Main()
    {
        if (!IsSelected)
            return DefaultAction;

        if (shootTimer.IsEnded && Input.GetKey(PlayerInputManager.Inst.Keys.MainAbility))
            return draw_AC;

        return main_AC;
    }
    private CLA_Action CL_Draw()
    {
        if (!IsSelected)
            return DefaultAction;

        if (!draw_AC.IsDrawing)
            return main_AC;

        return draw_AC;
    }
}
