using UnityEngine;

public class Player_Normal_Action : CLA_Action,
    ICanDetectGround
{
    #region Var: Inspector
    [Header("Ref")]
    [SerializeField] private Transform spriteRoot;
    [SerializeField] private BoxCollider2D oneWayCollider;

    [Header("Item PickUp")]
    [SerializeField] private PlayerItemPickUpData itemPickUpData;

    [Header("GroundDetection")]
    [SerializeField] private GroundDetectionData groundDetectionData;

    [Header("Gravity")]
    [SerializeField] private GravityData gravityData;

    [Header("Walk")]
    [SerializeField] private PlayerWalkData walkData;

    [Header("Jump")]
    [SerializeField] private JumpData jumpData;
    #endregion

    #region Var: Jump
    private bool jumpKeyPressed = false;
    private bool canPlayJumpAnim = true;
    #endregion

    #region Var: Fall Through
    private bool fallThroughKeyPressed = false;
    #endregion

    #region Var: Components
    private Animator animator;
    private Rigidbody2D rb2D;
    private SpriteRenderer bodySpriteRenderer;
    #endregion

    #region Var: Properties
    public JumpData JumpData => jumpData;
    #endregion

    #region Method: Unity
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        bodySpriteRenderer = spriteRoot.GetComponent<SpriteRenderer>();

        groundDetectionData.Size = oneWayCollider.size;
    }
    #endregion

    #region Method: CLA_Action
    public override void OnExit()
    {
        // Reset Ground Data
        groundDetectionData.isGrounded = false;

        // Reset Jump Data
        jumpData.isJumping = false;

        // Reset Velocity
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
    }
    public override void OnUpdate()
    {
        itemPickUpData.PickUp(transform, Input.GetKeyDown(KeyCode.LeftControl));
        PlayerItemDrop_Logic.DropFromHotbar(Input.GetKeyDown(KeyCode.Q));
    }
    public override void OnLateUpdate()
    {
        // Look At Mouse
        bodySpriteRenderer.LookAtMouseY(Global.Inst.MainCam, transform);
    }
    public override void OnFixedUpdate()
    {
        // Detect Ground
        groundDetectionData.DetectGround(!jumpData.isJumping, rb2D, transform);
        groundDetectionData.ExecuteOnGroundMethod(this);

        // Fall Through
        fallThroughKeyPressed = PlayerInputManager.Inst.Input_FallThrough;
        groundDetectionData.FallThrough(ref fallThroughKeyPressed, rb2D, transform, oneWayCollider);

        // Walk
        walkData.Walk(PlayerInputManager.Inst.Input_WalkDir, rb2D, jumpData.isJumping);

        // Jump
        jumpKeyPressed = PlayerInputManager.Inst.Input_Jump;
        jumpData.PlayerJump(ref jumpKeyPressed, rb2D, transform);

        // Gravity
        Gravity_Logic.ApplyGravity(rb2D, 
            groundDetectionData.isGrounded ? new GravityData(false, 0, 0) : 
            !jumpData.isJumping ? gravityData : 
            new GravityData(true, jumpData.jumpGravity, 0));

        // Animation
        UpdateAnimation();
    }
    #endregion

    #region Method: Animation
    private void UpdateAnimation()
    {
        const string
        Idle = "Player_Idle",
        Walk_Forward = "Player_Walk_Forward",
        Walk_Backward = "Player_Walk_Backward",
        Airborne = "Player_Airborne",
        Jump = "Player_Jump",
        AirJump = "Player_AirJump";

        string jumpAnim = jumpData.canJump ? Jump : AirJump;

        if (groundDetectionData.isGrounded)
        {
            if (PlayerInputManager.Inst.Input_WalkDir == 0)
            {
                animator.Play(Idle);
            }
            else
            {
                if ((PlayerInputManager.Inst.Input_WalkDir == 1 && spriteRoot.rotation.eulerAngles.y == 0) ||
                    (PlayerInputManager.Inst.Input_WalkDir == -1 && spriteRoot.rotation.eulerAngles.y == 180))
                    animator.Play(Walk_Forward);
                else
                    animator.Play(Walk_Backward);
            }
        }
        else
        {
            if (!jumpData.isJumping)
            {
                animator.Play(Airborne);
            }
            else if (canPlayJumpAnim)
            {
                animator.Play(jumpAnim);

                if (PlayerInputManager.Inst.Input_Jump)
                    animator.Play(jumpAnim);
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