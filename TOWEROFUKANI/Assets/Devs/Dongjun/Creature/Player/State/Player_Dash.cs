using UnityEngine;

public class Player_Dash : SSM_State_wMain<Player>
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

    #region Var: Properties
    public bool IsDasing { get; private set; } = false;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
    }
    #endregion

    #region Method: SSM
    public override void OnEnter()
    {
        // Set Value
        IsDasing = true;
        dashTime_Cur = 0;
        dashDir = PlayerInputManager.Inst.Input_DashDir;
        curTrailCount = 0;

        // Play Animation
        main.animator.Play(dashDir == main.Dir ? "Dash_Forward" : "Dash_Backward", 0, 0f);

        // Player Will Not Take Damage
        PlayerStats.Inst.AbsorbDamage = true;

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.DashStart);
    }
    public override void OnExit()
    {
        // Reset Velocity
        main.rb2D.velocity = new Vector2(0, 0);

        // Reset Value
        IsDasing = false;
        dashDir = 0;

        // Player Will Take Damage
        PlayerStats.Inst.AbsorbDamage = false;

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.DashEnd);
    }
    public override void OnFixedUpdate()
    {
        // Timer
        dashTime_Cur += Time.fixedDeltaTime;

        // Dash
        if (dashTime_Cur < dashTime)
        {
            main.rb2D.velocity = new Vector2(dashDir * (dashDist / dashTime), 0);

            // Trail Effect
            if (dashTime_Cur >= dashTime * (curTrailCount / trailCount))
            {
                curTrailCount++;
                main.bodySpriteRenderer.SpawnTrail(spriteTrailObject, trailDuration, main.bodySpriteRenderer.transform);
            }

            // Trigger Item Effect
            ActionEffectManager.Trigger(PlayerActions.Dashing);
        }
        else
        {
            IsDasing = false;
            return;
        }
    }
    #endregion
}
