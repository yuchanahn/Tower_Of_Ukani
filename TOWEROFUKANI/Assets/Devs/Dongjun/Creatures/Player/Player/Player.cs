using UnityEngine;

public class Player : SSM_Main
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
    #endregion

    #region Var: Properties
    public int Dir => bodySpriteRenderer.flipX ? -1 : 1;
    #endregion

    #region Var: CLA_Action
    private Player_Normal_Action state_Normal;
    private Player_Dash_Action state_Dash;
    private Player_Kick_Action state_Kick;
    #endregion

    #region Method: Init
    protected override void InitStates()
    {
        SetLogic(ref state_Normal, () => 
        {
            if (PlayerInputManager.Inst.Input_DashDir != 0 && PlayerStats.Inst.UseStamina(1))
                return state_Dash;

            if (Input.GetKeyDown(PlayerActionKeys.Kick))
                return state_Kick;

            return null;
        });

        SetLogic(ref state_Dash, () => 
        {
            // Cancle On Jump
            if (state_Normal.JumpData.canJump && PlayerInputManager.Inst.Input_Jump)
                return state_Normal;

            // Cancle On End
            if (!state_Dash.IsDasing)
                return state_Normal;

            return null;
        });

        SetLogic(ref state_Kick, () => 
        {
            if (!state_Kick.IsKicking)
                return state_Normal;

            return null;
        });

        SetDefaultState(state_Normal);
    }
    #endregion
}