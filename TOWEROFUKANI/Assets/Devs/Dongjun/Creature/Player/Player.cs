using Dongjun.Helper;
using UnityEngine;

public class Player : SSM_Main
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

    #region Var: Player
    [HideInInspector] public bool CanDash = true;
    [HideInInspector] public bool CanKick = true;
    [HideInInspector] public bool CanChangeDir = true;
    [HideInInspector] public bool PlayingOtherMotion = false;
    #endregion

    #region Var: Properties
    public StatusID statusID
    { get; private set; } = new StatusID();
    public Rigidbody2D rb2D
    { get; private set; }
    public Animator animator
    { get; private set; }

    public int Dir => bodySpriteRenderer.flipX ? -1 : 1;

    public bool IsDashing => CurrentState == state_Dash;
    public bool IsKicking => CurrentState == state_Kick;
    public bool IsOtherMotion => CurrentState == state_OtherMotion;
    #endregion

    #region Var: States
    private Player_HardCC state_HardCC;
    private Player_OtherMotion state_OtherMotion;
    private Player_Normal state_Normal;
    private Player_Dash state_Dash;
    private Player_Kick state_Kick;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        base.Awake();
    }
    #endregion

    #region Method: Init States
    protected override void InitStates()
    {
        SetLogic(When.AnyAction, () =>
        {
            if (PlayerStatus.IsHardCCed)
                return state_HardCC;

            if (!IsOtherMotion && PlayingOtherMotion)
                return state_OtherMotion;

            return null;
        });

        SetLogic(ref state_HardCC, () =>
        {
            if (!PlayerStatus.IsHardCCed)
                return state_Normal;

            return null;
        });

        SetLogic(ref state_OtherMotion, () => 
        {
            if (CanDash && PlayerInputManager.Inst.Input_DashDir != 0 && PlayerStats.Inst.UseStamina(1))
                return state_Dash;

            if (CanKick && Input.GetKeyDown(PlayerActionKeys.Kick))
                return state_Kick;

            if (!PlayingOtherMotion)
                return state_Normal;

            return null;
        });

        SetLogic(ref state_Normal, () => 
        {
            if (CanDash && PlayerInputManager.Inst.Input_DashDir != 0 && PlayerStats.Inst.UseStamina(1))
                return state_Dash;

            if (CanKick && Input.GetKeyDown(PlayerActionKeys.Kick))
                return state_Kick;

            return null;
        });

        SetLogic(ref state_Dash, () => 
        {
            // Cancle On Jump
            if (state_Normal.JumpData.canJump && PlayerInputManager.Inst.Input_Jump)
                return state_Normal;

            // Dash Done
            if (state_Dash.DashDone)
                return state_Normal;

            return null;
        });

        SetLogic(ref state_Kick, () => 
        {
            if (!state_Kick.IsKicking)
                return state_Normal;

            return null;
        });

        SetDefaultState(state_Normal);
    }
    #endregion
}