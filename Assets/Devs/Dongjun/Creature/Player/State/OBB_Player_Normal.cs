using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_Player_Normal : OBB_Player_State_Base,
    ICanDetectGround
{
    #region Var: Inspector
    [Header("Ref")]
    [SerializeField] private BoxCollider2D oneWayCollider;

    [Header("Item PickUp")]
    [SerializeField] private PlayerItemPickUpData itemPickUpData;

    [Header("Jump")]
    [SerializeField] private JumpCurveData jumpData;

    const int THE_CONST = 0;
    const int THE_CONST2 = 0;
    #endregion

    #region Var: Jump
    private bool canPlayJumpAnim = true;
    #endregion

    #region Var: Fall Through
    private bool fallThroughKeyPressed = false;
    #endregion

    #region Prop: 
    public JumpCurveData JumpData => jumpData;
    #endregion

    #region Method: Unity
    private void Start()
    {
        // Init Ground Detection Size
        data.groundDetectionData.Size = oneWayCollider.size;
    }
    #endregion

    #region Method: OBB
    public override void OnEnter()
    {
        if (data.groundDetectionData.isGrounded)
            jumpData.ResetJumpCount();
    }
    public override void OnExit()
    {
        // Reset Ground Data
        data.groundDetectionData.isGrounded = false;

        // Reset Jump Data
        jumpData.isJumping = false;
    }
    public override void OnUpdate()
    {
        // Pick Up Item
        if (Input.GetKeyDown(PlayerActionKeys.PickUpItem))
            itemPickUpData.PickUp(transform);
    }
    public override void OnLateUpdate()
    {
        // Character Body Look At Mouse
        if (data.CanChangeDir)
            data.bodySpriteRenderer.LookAtMouseFlipX(CamManager.Inst.MainCam, transform);
    }
    public override void OnFixedUpdate()
    {
        // Detect Ground
        data.groundDetectionData.DetectGround(!jumpData.isJumping, data.RB2D, transform);
        data.groundDetectionData.ExecuteOnGroundMethod(this);

        // Fall Through
        fallThroughKeyPressed = PlayerInputManager.Inst.Input_FallThrough;
        data.groundDetectionData.FallThrough(ref fallThroughKeyPressed, data.RB2D, transform, oneWayCollider);

        // Walk
        PlayerStats.Inst.walkData.Walk(PlayerInputManager.Inst.Input_WalkDir, data.RB2D, jumpData.isJumping);

        // Follow Moving Platform
        data.groundDetectionData.FollowMovingPlatform(data.RB2D);

        // Jump
        if (jumpData.Jump(ref PlayerInputManager.Inst.Input_Jump, data.RB2D, transform))
            PlayerActionEventManager.Trigger(PlayerActions.Jump);

        // Gravity
        Gravity_Logic.ApplyGravity(data.RB2D,
            data.groundDetectionData.isGrounded || jumpData.isJumping
            ? GravityData.Zero
            : data.gravityData);

        // Update Animation
        UpdateAnimation();
    }
    #endregion

    #region Method: Animation
    private void UpdateAnimation()
    {
        const string
            IDLE = "Idle",
            WALK_FORWARD = "Walk_Forward",
            WALK_BACKWARD = "Walk_Backward",
            AIRBORNE = "Airborne",
            JUMP = "Jump",
            AIRJUMP = "AirJump";

        // 땅위에 있을 때
        if (data.groundDetectionData.isGrounded)
        {
            data.Animator.Play(
                PlayerInputManager.Inst.Input_WalkDir == 0 ? IDLE :
                PlayerInputManager.Inst.Input_WalkDir == data.Dir ? WALK_FORWARD :
                WALK_BACKWARD);
        }
        else
        {
            if (!jumpData.isJumping)
            {
                data.Animator.Play(AIRBORNE);
            }
            else if (canPlayJumpAnim)
            {
                data.Animator.Play(jumpData.CanJump ? JUMP : AIRJUMP);
            }
            canPlayJumpAnim = jumpData.CanJump;
        }
    }
    #endregion

    #region Interface: ICanDetectGround
    void ICanDetectGround.OnGroundEnter()
    {
        // Reset Jump
        jumpData.ResetJumpCount();
    }
    void ICanDetectGround.OnGroundStay()
    {
    }
    void ICanDetectGround.OnGroundExit()
    {
    }
    #endregion
}
