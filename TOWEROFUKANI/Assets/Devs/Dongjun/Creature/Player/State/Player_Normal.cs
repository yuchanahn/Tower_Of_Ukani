using UnityEngine;

public class Player_Normal : SSM_State_wMain<Player>,
    ICanDetectGround
{
    #region Var: Inspector
    [Header("Ref")]
    [SerializeField] private BoxCollider2D oneWayCollider;

    [Header("Item PickUp")]
    [SerializeField] private PlayerItemPickUpData itemPickUpData;

    [Header("Jump")]
    [SerializeField] private JumpData jumpData;

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
    public JumpData JumpData => jumpData;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        // Init Ground Detection Size
        main.groundDetectionData.Size = oneWayCollider.size;
    }
    #endregion

    #region Method: SSM
    public override void OnEnter()
    {
        if (main.groundDetectionData.isGrounded)
            jumpData.ResetJumpCount();
    }
    public override void OnExit()
    {
        // Reset Ground Data
        main.groundDetectionData.isGrounded = false;

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
        if (main.CanChangeDir)
            main.bodySpriteRenderer.LookAtMouseFlipX(Global.Inst.MainCam, transform);
    }
    public override void OnFixedUpdate()
    {
        // Detect Ground
        main.groundDetectionData.DetectGround(!jumpData.isJumping, main.RB2D, transform);
        main.groundDetectionData.ExecuteOnGroundMethod(this);

        // Fall Through
        fallThroughKeyPressed = PlayerInputManager.Inst.Input_FallThrough;
        main.groundDetectionData.FallThrough(ref fallThroughKeyPressed, main.RB2D, transform, oneWayCollider);

        // Walk
        PlayerStats.Inst.walkData.Walk(PlayerInputManager.Inst.Input_WalkDir, main.RB2D, jumpData.isJumping);

        // Follow Moving Platform
        main.groundDetectionData.FollowMovingPlatform(main.RB2D);

        // Jump
        if (jumpData.Jump(ref PlayerInputManager.Inst.Input_Jump, main.RB2D, transform))
            ActionEffectManager.Trigger(PlayerActions.Jump);

        // Gravity
        Gravity_Logic.ApplyGravity(main.RB2D,
            main.groundDetectionData.isGrounded ? GravityData.Zero : 
            !jumpData.isJumping ? main.gravityData : 
            new GravityData(accel: jumpData.jumpGravity));

        // Update Animation
        UpdateAnimation();
    }
    #endregion

    #region Method: Animation
    private void UpdateAnimation()
    {
        const string
            IDLE           = "Idle",
            WALK_FORWARD   = "Walk_Forward",
            WALK_BACKWARD  = "Walk_Backward",
            AIRBORNE       = "Airborne",
            JUMP           = "Jump",
            AIRJUMP        = "AirJump";

        // 땅위에 있을 때
        if (main.groundDetectionData.isGrounded)
        {
            main.Animator.Play(
                PlayerInputManager.Inst.Input_WalkDir == 0 ? IDLE :
                PlayerInputManager.Inst.Input_WalkDir == main.Dir ? WALK_FORWARD :
                WALK_BACKWARD);
        }
        else
        {
            if (!jumpData.isJumping)
            {
                main.Animator.Play(AIRBORNE);
            }
            else if (canPlayJumpAnim)
            {
                main.Animator.Play(jumpData.canJump ? JUMP : AIRJUMP);
            }
            canPlayJumpAnim = jumpData.canJump;
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