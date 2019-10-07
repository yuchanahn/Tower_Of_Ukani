using UnityEngine;

public static class PlayerMovementKeys
{
    public static KeyCode WalkRight => KeyCode.D;
    public static KeyCode WalkLeft => KeyCode.A;
    public static KeyCode FallThrough => KeyCode.S;
    public static KeyCode Jump => KeyCode.Space;
}

public class PlayerActionKeys
{
    public static KeyCode Dash => KeyCode.LeftShift;
    public static KeyCode Kick => KeyCode.LeftControl;
}

public class PlayerWeaponKeys
{
    public static KeyCode MainAbility => KeyCode.Mouse0;
    public static KeyCode SubAbility => KeyCode.Mouse1;
    public static KeyCode SpecialAbility => KeyCode.F;
    public static KeyCode Reload => KeyCode.R;
}

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Inst { get; private set; }

    #region Var: Walk
    public int Input_WalkDir { get; private set; } = 0;
    private int input_walkRight = 0;
    private int input_walkLeft = 0;
    #endregion

    #region Var: FallThrough
    public bool Input_FallThrough { get; private set; } = false;
    #endregion

    #region Var: Jump
    public bool Input_Jump { get; private set; } = false;
    #endregion

    #region Var: Dash
    [HideInInspector]
    public int Input_DashDir = 0;
    private int oldDashInput = 0;

    private readonly float dashInputInterval = 0.2f;
    private float dashInputTime = 0;
    private int dashInputCount = 0;
    #endregion


    #region Method: Unity
    private void Awake()
    {
        Inst = this;
    }
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
    }
    #endregion

    #region Method: Walk
    private void GetInput_Walk()
    {
        if (Input.GetKeyDown(PlayerMovementKeys.WalkRight)) input_walkRight = input_walkLeft + 1;
        if (Input.GetKeyDown(PlayerMovementKeys.WalkLeft)) input_walkLeft = input_walkRight + 1;

        if (Input.GetKeyUp(PlayerMovementKeys.WalkRight)) input_walkRight = 0;
        if (Input.GetKeyUp(PlayerMovementKeys.WalkLeft)) input_walkLeft = 0;

        Input_WalkDir = input_walkRight == 0 && input_walkLeft == 0 ? 0 : 
                        input_walkRight > input_walkLeft ? 1 : 
                        -1;
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
