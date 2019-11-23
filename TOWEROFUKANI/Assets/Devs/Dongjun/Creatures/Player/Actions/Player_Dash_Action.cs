using UnityEngine;

public class Player_Dash_Action : CLA_Action<Player>
{
    #region Var: Inspector
    [Header("Dash")]
    [SerializeField] private float dashDist;
    [SerializeField] private float dashTime;

    [Header("Effect")]
    [SerializeField] private SpriteTrailObject spriteTrailObject;
    [SerializeField] private float trailDuration = 0.15f;
    [SerializeField] private float trailCount = 4;
    #endregion

    #region Var: Dash
    private float dashTime_Cur = 0;
    private int dashDir = 0;
    #endregion

    #region Var: Effect
    int curTrailCount = 0;
    #endregion

    #region Var: Components
    private Animator animator;
    private Rigidbody2D rb2D;
    #endregion

    #region Var: Properties
    public bool IsDasing { get; private set; } = false;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    #endregion

    #region Method: CLA_Action
    public override void OnEnter()
    {
        // Set Value
        IsDasing = true;
        dashTime_Cur = 0;
        dashDir = PlayerInputManager.Inst.Input_DashDir;
        curTrailCount = 0;

        // Play Animation
        animator.Play(dashDir == main.Dir ? "Player_Dash_Forward" : "Player_Dash_Backward", 0, 0f);

        // Trigger Item Effect
        ItemEffectManager.Trigger(PlayerActions.Dash);
    }
    public override void OnExit()
    {
        // Reset Velocity
        rb2D.velocity = new Vector2(0, 0);

        // Reset Value
        IsDasing = false;
        dashDir = 0;
    }
    public override void OnFixedUpdate()
    {
        // Timer
        dashTime_Cur += Time.fixedDeltaTime;

        // Dash
        if (dashTime_Cur >= dashTime)
            IsDasing = false;
        else
            rb2D.velocity = new Vector2(dashDir * (dashDist / dashTime), 0);

        // Trail Effect
        if (dashTime_Cur >= dashTime * (curTrailCount / trailCount))
        {
            curTrailCount++;
            main.bodySpriteRenderer.SpawnTrail(spriteTrailObject, trailDuration, transform);
        }
    }
    #endregion
}
