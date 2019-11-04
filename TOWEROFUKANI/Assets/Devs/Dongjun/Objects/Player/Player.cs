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
    #endregion

    #region Var: Properties
    public int Dir => bodySpriteRenderer.flipX ? -1 : 1;
    #endregion

    #region Var: CLA_Action
    private Player_Normal_Action normal_AC;
    private Player_Dash_Action dash_AC;
    private Player_Kick_Action kick_AC;
    #endregion

    #region Method: Init
    protected override void Init()
    {
        normal_AC = GetComponent<Player_Normal_Action>();
        dash_AC = GetComponent<Player_Dash_Action>();
        kick_AC = GetComponent<Player_Kick_Action>();

        ConditionLogics.Add(normal_AC, CL_Normal);
        ConditionLogics.Add(dash_AC, CL_Dash);
        ConditionLogics.Add(kick_AC, CL_Kick);
    }
    #endregion
    
    #region Method: Condition Logic
    private CLA_Action CL_Normal()
    {
        if (PlayerInputManager.Inst.Input_DashDir != 0 && PlayerStatUIManager.UseStamina())
            return dash_AC;

        if (Input.GetKeyDown(PlayerActionKeys.Kick))
            return kick_AC;

        return normal_AC;
    }
    private CLA_Action CL_Dash()
    {
        // Cancle On Jump
        if (normal_AC.JumpData.canJump && PlayerInputManager.Inst.Input_Jump)
            return normal_AC;

        // Cancle On End
        if (!dash_AC.IsDasing)
            return normal_AC;

        return dash_AC;
    }
    private CLA_Action CL_Kick()
    {
        if (!kick_AC.IsKicking)
            return normal_AC;

        return kick_AC;
    }
    #endregion
}