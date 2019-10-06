using System.Collections.Generic;
using UnityEngine;

public class Keys
{
    // Movement
    public KeyCode WalkRight => KeyCode.D;
    public KeyCode WalkLeft => KeyCode.A;
    public KeyCode FallThrough => KeyCode.S;
    public KeyCode Jump => KeyCode.Space;

    // Action
    public KeyCode Dash => KeyCode.LeftShift;
    public KeyCode Kick => KeyCode.LeftControl;

    // Weapon
    public KeyCode MainAbility => KeyCode.Mouse0;
    public KeyCode SubAbility => KeyCode.Mouse1;
    public KeyCode SpecialAbility => KeyCode.F;
    public KeyCode Reload => KeyCode.R;
}

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Inst { get; private set; }

    #region Var: KeyCode
    public Keys Keys { get; private set; } = new Keys();
    #endregion

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
        if (Input.GetKeyDown(Keys.WalkRight)) input_walkRight = input_walkLeft + 1;
        if (Input.GetKeyDown(Keys.WalkLeft)) input_walkLeft = input_walkRight + 1;

        if (Input.GetKeyUp(Keys.WalkRight)) input_walkRight = 0;
        if (Input.GetKeyUp(Keys.WalkLeft)) input_walkLeft = 0;

        Input_WalkDir = input_walkRight == 0 && input_walkLeft == 0 ? 0 : 
                        input_walkRight > input_walkLeft ? 1 : 
                        -1;
    }
    #endregion

    #region Method: FallThrough
    private void GetInput_FallThrough()
    {
        if (Input.GetKeyDown(Keys.FallThrough))
            Input_FallThrough = true;
    }
    #endregion

    #region Method: Jump
    private void GetInput_Jump()
    {
        if (Input.GetKeyDown(Keys.Jump))
            Input_Jump = true;
    }
    #endregion

    #region Method: Dash
    private void GetInput_Dash()
    {
        if (Input.GetKeyDown(Keys.Dash))
        {
            Input_DashDir = Input_WalkDir;
            dashInputCount = 0;
            dashInputTime = 0;
            return;
        }

        if (Input.GetKeyDown(Keys.WalkRight) || 
            Input.GetKeyDown(Keys.WalkLeft))
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
