using UnityEngine;

public sealed class PlayerInputManager : SingletonBase<PlayerInputManager>
{
    #region Var: Walk
    private int input_WalkDir = 0;
    public int Input_WalkDir => input_WalkDir;
    #endregion

    #region Var: FallThrough
    public bool Input_FallThrough { get; private set; } = false;
    #endregion

    #region Var: Jump
    public bool Input_Jump { get; private set; } = false;
    #endregion

    #region Var: Dash
    [HideInInspector]
    private readonly float dashInputInterval = 0.2f;
    private float dashInputTime = 0;
    private int dashInputCount = 0;
    private int oldDashInput = 0;
    public int Input_DashDir
    { get; private set; }
    #endregion

    #region Method: Unity
    private void Update()
    {
        // Movement
        GetInput_Walk();
        GetInput_FallThrough();
        GetInput_Jump();

        // Ability
        GetInput_Dash();
    }
    private void FixedUpdate()
    {
        Input_Jump = false;
        Input_FallThrough = false;
        Input_DashDir = 0;
    }
    #endregion

    #region Method: Walk
    private void GetInput_Walk()
    {
        void GetDir(ref int dir, KeyCode plusKey, KeyCode minusKey)
        {
            if (Input.GetKeyDown(plusKey))
                dir = 1;
            if (Input.GetKeyDown(minusKey))
                dir = -1;

            if (dir == 1 && Input.GetKeyUp(plusKey))
                dir = -1;
            if (dir == -1 && Input.GetKeyUp(minusKey))
                dir = 1;

            if (!Input.GetKey(plusKey) && !Input.GetKey(minusKey))
                dir = 0;
        }

        GetDir(ref input_WalkDir, PlayerMovementKeys.WalkRight, PlayerMovementKeys.WalkLeft);
    }
    #endregion

    #region Method: FallThrough
    private void GetInput_FallThrough()
    {
        if (Input.GetKeyDown(PlayerMovementKeys.FallThrough))
            Input_FallThrough = true;
    }
    #endregion

    #region Method: Jump
    private void GetInput_Jump()
    {
        if (Input.GetKeyDown(PlayerMovementKeys.Jump))
            Input_Jump = true;
    }
    #endregion

    #region Method: Dash
    private void GetInput_Dash()
    {
        if (Input.GetKeyDown(PlayerActionKeys.Dash))
        {
            Input_DashDir = Input_WalkDir;
            dashInputCount = 0;
            dashInputTime = 0;
            return;
        }

        if (Input.GetKeyDown(PlayerMovementKeys.WalkRight) || 
            Input.GetKeyDown(PlayerMovementKeys.WalkLeft))
        {
            // First Tap
            if (dashInputCount == 0)
                dashInputCount++;

            // After First Tap
            if (oldDashInput == Input_WalkDir && dashInputTime <= dashInputInterval)
                dashInputCount++;
            else
                dashInputCount = 0;

            // Check Tap Count
            if (dashInputCount == 2)
            {
                Input_DashDir = Input_WalkDir;
                dashInputCount = 0;
            }

            // Reset Data
            dashInputTime = 0;
            oldDashInput = Input_WalkDir;
        }

        dashInputTime += Time.deltaTime;
    }
    #endregion
}
