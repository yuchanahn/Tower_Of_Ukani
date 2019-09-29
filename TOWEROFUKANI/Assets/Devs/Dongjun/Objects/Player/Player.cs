using UnityEngine;

public class Player : CLA_Main
{
    private Player_Movement_Action movement_Action;
    private Player_Dash_Action dash_Action;

    protected override void Init()
    {
        movement_Action = GetComponent<Player_Movement_Action>();
        dash_Action = GetComponent<Player_Dash_Action>();

        ConditionLogics.Add(movement_Action, CL_Movement);
        ConditionLogics.Add(dash_Action, CL_Dash);
    }

    private void CL_Movement()
    {
        if (PlayerInputManager.Inst.Input_DashDir != 0)
            ChangeAction(dash_Action);
    }
    private void CL_Dash()
    {
        if (movement_Action.JumpData.canJump && PlayerInputManager.Inst.Input_Jump)
            ChangeAction(movement_Action);

        if (!dash_Action.IsDasing)
            ChangeAction(movement_Action);
    }
}