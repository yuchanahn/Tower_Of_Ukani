using UnityEngine;

public class Player_Normal : SSM_State_wMain<Player>,
    ICanDetectGround
{
    #region Var: Inspector
    [Header("Ref")]
    [SerializeField] private BoxCollider2D oneWayCollider;

    [Header("Item PickUp")]
    [SerializeField] private PlayerItemPickUpData itemPickUpData;

    [Header("Walk")]
    [SerializeField] private PlayerWalkData walkData;

    [Header("Jump")]
    [SerializeField] private JumpData jumpData;
    #endregion

    #region Var: Jump
    private bool canPlayJumpAnim = true;
    #endregion

    #region Var: Fall Through
    private bool fallThroughKeyPressed = false;
    #endregion

    #region Var: Properties
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
        main.groundDetectionData.DetectGround(!jumpData.isJumping, main.rb2D, transform);
        main.groundDetectionData.ExecuteOnGroundMethod(this);

        // Fall Through
        fallThroughKeyPressed = PlayerInputManager.Inst.Input_FallThrough;
        main.groundDetectionData.FallThrough(ref fallThroughKeyPressed, main.rb2D, transform, oneWayCollider);

        // Walk
        walkData.Walk(PlayerInputManager.Inst.Input_WalkDir, main.rb2D, jumpData.isJumping);

        // Follow Moving Platform
        main.groundDetectionData.FollowMovingPlatform(main.rb2D);

        // Jump
        if (jumpData.Jump(ref PlayerInputManager.Inst.Input_Jump, main.rb2D, transform))
            ActionEffectManager.Trigger(PlayerActions.Jump);

        // Gravity
        Gravity_Logic.ApplyGravity(main.rb2D,
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
            Idle           = "Idle",
            Walk_Forward   = "Walk_Forward",
            Walk_Backward  = "Walk_Backward",
            Airborne       = "Airborne",
            Jump           = "Jump",
            AirJump        = "AirJump";

        // 땅위에 있을 때
        if (main.groundDetectionData.isGrounded)
        {
            main.animator.Play(
                PlayerInputManager.Inst.Input_WalkDir == 0 ? Idle :
                PlayerInputManager.Inst.Input_WalkDir == main.Dir ? Walk_Forward :
                Walk_Backward);
        }
        else
        {
            if (!jumpData.isJumping)
            {
                main.animator.Play(Airborne);
            }
            else if (canPlayJumpAnim)
            {
                main.animator.Play(jumpData.canJump ? Jump : AirJump);
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