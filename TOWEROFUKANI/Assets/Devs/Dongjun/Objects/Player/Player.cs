using UnityEngine;

public class Player : CLA_Main
{
    #region Var: CLA_Action
    private Player_Normal_Action movement_AC;
    private Player_Dash_Action dash_AC;
    #endregion

    #region Method: Init
    protected override void Init()
    {
        movement_AC = GetComponent<Player_Normal_Action>();
        dash_AC = GetComponent<Player_Dash_Action>();

        ConditionLogics.Add(movement_AC, CL_Movement);
        ConditionLogics.Add(dash_AC, CL_Dash);
    }
    #endregion

    
    #region Method: Condition Logic
    private CLA_Action CL_Movement()
    {
        if (PlayerInputManager.Inst.Input_DashDir != 0 && PlayerStatUIManager.UseStamina())
            return dash_AC;

        return movement_AC;
    }
    private CLA_Action CL_Dash()
    {
        // Cancle On Jump
        if (movement_AC.JumpData.canJump && PlayerInputManager.Inst.Input_Jump)
            return movement_AC;

        // Cancle On End
        if (!dash_AC.IsDasing)
            return movement_AC;

        return dash_AC;
    }
    #endregion
}