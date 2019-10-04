using UnityEngine;

public class Player : CLA_Main
{
    private Player_Movement_Action movement_AC;
    private Player_Dash_Action dash_AC;

    protected override void Init()
    {
        movement_AC = GetComponent<Player_Movement_Action>();
        dash_AC = GetComponent<Player_Dash_Action>();

        ConditionLogics.Add(movement_AC, CL_Movement);
        ConditionLogics.Add(dash_AC, CL_Dash);
    }

    private CLA_Action CL_Movement()
    {
        if (PlayerInputManager.Inst.Input_DashDir != 0)
            return dash_AC;

        return movement_AC;
    }
    private CLA_Action CL_Dash()
    {
        if (movement_AC.JumpData.canJump && PlayerInputManager.Inst.Input_Jump)
            return movement_AC;

        if (!dash_AC.IsDasing)
            return movement_AC;

        return dash_AC;
    }
}