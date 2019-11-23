using UnityEngine;

public class Player : CLA_Main
{
    #region Var: Inspector
    [Header("Component")]
    public Transform spriteRoot;
    public SpriteRenderer bodySpriteRenderer;
    public BoxCollider2D oneWayCollider;

    [Header("GroundDetection")]
    public GroundDetectionData groundDetectionData;

    [Header("Gravity")]
    public GravityData gravityData;

    [Header("Effect")]
    public SpriteTrailObject spriteTrailObject;
    #endregion

    #region Var: Properties
    public int Dir => bodySpriteRenderer.flipX ? -1 : 1;
    #endregion

    #region Var: CLA_Action
    private Player_Normal_Action action_Normal;
    private Player_Dash_Action action_Dash;
    private Player_Kick_Action action_Kick;
    #endregion

    #region Method: Init
    protected override void Init()
    {
        AddLogic(ref action_Normal, CL_Normal);
        AddLogic(ref action_Dash, CL_Dash);
        AddLogic(ref action_Kick, CL_Kick);
    }
    #endregion
    
    #region Method: Condition Logic
    private CLA_Action_Base CL_Normal()
    {
        if (PlayerInputManager.Inst.Input_DashDir != 0 && PlayerStats.UseStamina(1))
            return action_Dash;

        if (Input.GetKeyDown(PlayerActionKeys.Kick))
            return action_Kick;

        return action_Normal;
    }
    private CLA_Action_Base CL_Dash()
    {
        // Cancle On Jump
        if (action_Normal.JumpData.canJump && PlayerInputManager.Inst.Input_Jump)
            return action_Normal;

        // Cancle On End
        if (!action_Dash.IsDasing)
            return action_Normal;

        return action_Dash;
    }
    private CLA_Action_Base CL_Kick()
    {
        if (!action_Kick.IsKicking)
            return action_Normal;

        return action_Kick;
    }
    #endregion
}