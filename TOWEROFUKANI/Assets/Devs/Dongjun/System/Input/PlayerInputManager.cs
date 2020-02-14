using UnityEngine;

public sealed class PlayerInputManager : SingletonBase<PlayerInputManager>
{
    #region Var: Walk
    private int input_WalkDir = 0;
    public int Input_WalkDir => input_WalkDir;
    #endregion

    #region Var: Jump
    public bool Input_Jump = false;
    #endregion

    #region Var: FallThrough
    public bool Input_FallThrough
    { get; private set; } = false;
    #endregion

    #region Var: Dash
    private readonly int dashTapCount = 2;
    private readonly float dashInputInterval = 0.2f;

    private float dashInputTime = 0;
    private int curDashTapCount = 0;
    private int prevDashInputDir = 0;

    public int Input_DashDir
    { get; private set; }
    #endregion

    #region Var: Weapon
    public bool CanUseWeapon
    { get; private set; } = true;
    #endregion

    #region Method: Unity
    private void Update()
    {
        // Movement
        GetInput_Walk();
        GetInput_Jump();
        GetInput_FallThrough();

        // Action
        GetInput_Dash();

        // Weapon
        UpdateWeaponKey();
    }
    #endregion

    #region Method: Reset Input
    public void ResetInput()
    {
        // Hmm.....
        if (Input.GetKeyUp(PlayerMovementKeys.Jump))
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

            if (Input.GetKey(minusKey) && Input.GetKeyUp(plusKey))
                dir = -1;
            if (Input.GetKey(plusKey) && Input.GetKeyUp(minusKey))
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
        // 대쉬 키를 눌렀을 때
        if (Input.GetKeyDown(PlayerMovementKeys.Dash))
        {
            Input_DashDir = Input_WalkDir;
            curDashTapCount = 0;
            dashInputTime = 0;
            return;
        }

        // 더블 탭 했을 때
        if (Input.GetKeyDown(PlayerMovementKeys.WalkRight) 
        || Input.GetKeyDown(PlayerMovementKeys.WalkLeft))
        {
            // First Tap
            if (curDashTapCount == 0)
                curDashTapCount = 1;

            // After First Tap
            curDashTapCount = 
                (prevDashInputDir == Input_WalkDir && dashInputTime <= dashInputInterval)
                ? curDashTapCount++
                : 0;

            prevDashInputDir = Input_WalkDir;
            dashInputTime = 0;

            // Check Tap Count
            if (curDashTapCount == dashTapCount)
            {
                curDashTapCount = 0;
                Input_DashDir = Input_WalkDir;
                return;
            }

            return;
        }

        dashInputTime += Time.deltaTime;
    }
    #endregion

    #region Method: Weapon
    private void UpdateWeaponKey()
    {
        // UI 위에서 무기 버튼을 누르면 무기 사용 못함.
        if (UI_Utility.IsMouseOverUI() 
        && (Input.GetKeyDown(PlayerWeaponKeys.MainAbility) || Input.GetKeyDown(PlayerWeaponKeys.SubAbility)))
        {
            CanUseWeapon = false;
            return;
        }

        // UI 위에서 무기 버튼을 눌렀을 때 이후에도 버튼을 계속 누르고 있으면 무기 사용 못함.
        if (!CanUseWeapon && Input.GetKey(PlayerWeaponKeys.MainAbility))
            return;

        // 다시 무기 사용 가능.
        CanUseWeapon = true;
    }
    #endregion
}
