using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OBB_Data_Player : OBB_Data_Animator,
    Creature
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

    // State
    [HideInInspector] public bool CanDash = true;
    [HideInInspector] public bool CanKick = true;
    [HideInInspector] public bool CanChangeDir = true;
    [HideInInspector] public bool PlayingOtherMotion = false;
    [HideInInspector] public bool NewKnockback = false;

    // Data
    public CreatureType CreatureType => CreatureType.Player;
    public StatusID StatusID
    { get; private set; } = new StatusID();
    public int Dir => bodySpriteRenderer.flipX ? -1 : 1;

    // Component
    public Rigidbody2D RB2D
    { get; private set; }

    public override void Init_Awake(GameObject gameObject)
    {
        base.Init_Awake(gameObject);
        RB2D = gameObject.GetComponent<Rigidbody2D>();
    }
    public override void Init_Start(GameObject gameObject)
    {
        base.Init_Start(gameObject);
        PlayerActionEventManager.AddEvent(PlayerActions.Knockbacked, this.NewPlayerActionEvent(() => NewKnockback = true));
    }
}

public class OBB_Player : OBB_Controller<OBB_Data_Player, OBB_Player_State_Base>
{
    [SerializeField] private AnimationCurve testCurve;

    // States
    private OBB_Player_Normal state_Normal;
    private OBB_Player_Incapacitated state_Incapacitated;
    private OBB_Player_OtherMotion state_OtherMotion;
    private OBB_Player_Dash state_Dash;
    private OBB_Player_Kick state_Kick;

    // Behaviours
    private Single bvr_Normal;
    private Single bvr_Incapacitated;
    private Single bvr_OtherMotion;
    private Single bvr_Dash;
    private Single bvr_Kick;

    public bool IsDashing => (object)currentState == state_Dash;
    public bool IsKicking => (object)currentState == state_Kick;

    protected override void InitStates()
    {
        GetState(ref state_Normal);
        GetState(ref state_Incapacitated);
        GetState(ref state_OtherMotion);
        GetState(ref state_Dash);
        GetState(ref state_Kick);
        SetDefaultState(state_Normal, EMPTY_STATE_ACTION);
    }
    protected override void InitBehaviours()
    {
        bvr_Normal = new Single(
            state_Normal,
            EMPTY_STATE_ACTION,
            () => false);

        bvr_Incapacitated = new Single(
            state_Incapacitated,
            EMPTY_STATE_ACTION,
            () => !PlayerStatus.Incapacitated);

        bvr_OtherMotion = new Single(
            state_OtherMotion,
            EMPTY_STATE_ACTION,
            () => !Data.PlayingOtherMotion);

        bvr_Dash = new Single(
            state_Dash,
            EMPTY_STATE_ACTION,
            () => state_Dash.DashDone 
               || (state_Normal.JumpData.CanJump && PlayerInputManager.Inst.Input_Jump));

        bvr_Kick = new Single(
            state_Kick,
            EMPTY_STATE_ACTION,
            () => state_Kick.KickDone);
    }
    protected override void InitObjectives()
    {
        // Incapacitated
        NewObjective(
            () => PlayerStatus.Incapacitated, true)
            .AddBehaviour(bvr_Incapacitated, true);

        // Other Motion
        NewObjective(
            () => Data.PlayingOtherMotion, true)
            .AddBehaviour(bvr_OtherMotion, true);

        // Dash
        NewObjective(
            () => Data.CanDash
               && !IsDashing
               && PlayerInputManager.Inst.Input_DashDir != 0 
               && PlayerStats.Inst.UseStamina(1))
            .AddBehaviour(bvr_Dash, true);

        // Kick
        NewObjective(
            () => Data.CanKick 
               && Input.GetKeyDown(PlayerActionKeys.Kick))
            .AddBehaviour(bvr_Kick, true);

        // Default
        SetDefaultObjective()
            .AddBehaviour(bvr_Normal);
    }
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerStatus.AddEffect(new PlayerStatus_Knockback(Data.StatusID, gameObject, KnockbackMode.Weak, true, new Vector2(1, 1), testCurve));
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerStatus.AddEffect(new PlayerStatus_Stun(Data.StatusID, gameObject, 0.5f));
        }
    }
}
