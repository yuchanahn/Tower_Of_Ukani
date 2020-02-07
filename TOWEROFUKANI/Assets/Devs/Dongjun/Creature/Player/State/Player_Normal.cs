using UnityEngine;

public class Player_Normal : SSM_State_wMain<Player>,
    ICanDetectGround
{
    #region Var: Inspector
    [Header("Ref")]
    [SerializeField] private Transform spriteRoot;
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

    #region Var: Components
    private Animator animator;
    private Rigidbody2D rb2D;
    #endregion

    #region Var: Properties
    public JumpData JumpData => jumpData;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        main.groundDetectionData.Size = oneWayCollider.size;
    }
    #endregion

    #region Method: CLA_Action
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
        main.bodySpriteRenderer.LookAtMouseFlipX(Global.Inst.MainCam, transform);
    }
    public override void OnFixedUpdate()
    {
        // Detect Ground
        main.groundDetectionData.DetectGround(!jumpData.isJumping, rb2D, transform);
        main.groundDetectionData.ExecuteOnGroundMethod(this);

        // Fall Through
        fallThroughKeyPressed = PlayerInputManager.Inst.Input_FallThrough;
        main.groundDetectionData.FallThrough(ref fallThroughKeyPressed, rb2D, transform, oneWayCollider);

        // Walk
        walkData.Walk(PlayerInputManager.Inst.Input_WalkDir, rb2D, jumpData.isJumping);

        // Follow Moving Platform
        main.groundDetectionData.FollowMovingPlatform(rb2D);

        // Jump
        if (jumpData.Jump(ref PlayerInputManager.Inst.Input_Jump, rb2D, transform))
            ActionEffectManager.Trigger(PlayerActions.Jump);

        // Gravity
        Gravity_Logic.ApplyGravity(rb2D,
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
            animator.Play(
                PlayerInputManager.Inst.Input_WalkDir == 0 ? Idle :
                PlayerInputManager.Inst.Input_WalkDir == main.Dir ? Walk_Forward :
                Walk_Backward);
        }
        else
        {
            if (!jumpData.isJumping)
            {
                animator.Play(Airborne);
            }
            else if (canPlayJumpAnim)
            {
                animator.Play(jumpData.canJump ? Jump : AirJump);
            }
            canPlayJumpAnim = jumpData.canJump;
        }
    }
    #endregion

    #region Interface: ICanDetectGround
    public void OnGroundEnter()
    {
        // Reset Jump
        Jump_Logic.ResetJumpCount(ref jumpData);
    }
    public void OnGroundStay()
    {
    }
    public void OnGroundExit()
    {
    }
#endregion
}