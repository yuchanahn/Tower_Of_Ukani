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
    private bool isUsingMelee = false;
    #endregion

    #region Var: Status
    private PlayerStatus_IgnoreDamage status_IgnoreDamage;
    #endregion

    #region Var: Effect
    int curTrailCount = 0;
    #endregion

    #region Var: Properties
    public bool DashDone { get; private set; } = false;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        status_IgnoreDamage = new PlayerStatus_IgnoreDamage(GM.Player.StatusID, GM.Player.gameObject);
    }
    #endregion

    #region Method: SSM
    public override void OnEnter()
    {
        // Init Value
        dashDir = PlayerInputManager.Inst.Input_DashDir;

        // Status Effect
        PlayerStatus.AddEffect(status_IgnoreDamage);

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.DashStart);
    }
    public override void OnExit()
    {
        // Reset Value
        DashDone = false;
        dashDir = 0;
        dashTime_Cur = 0;
        curTrailCount = 0;

        // Reset Vel
        main.RB2D.velocity = new Vector2(0, 0);

        // Status Effect
        PlayerStatus.RemoveEffect(status_IgnoreDamage);

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.DashEnd);

        // When Using Melee Weapon
        if (isUsingMelee)
        {
            isUsingMelee = false;

            // Unlock Slot
            PlayerInventoryManager.weaponHotbar.LockSlots(this, false);
        }
    }
    public override void OnUpdate()
    {
        if (!isUsingMelee)
        {
            // When Using Melee Weapon
            isUsingMelee = PlayerInventoryManager.weaponHotbar.CurWeapon == null || PlayerInventoryManager.weaponHotbar.CurWeapon is MeleeItem;
            if (isUsingMelee)
            {
                // Look at Dash Dir
                main.bodySpriteRenderer.flipX = dashDir == 1 ? false : true;

                // Lock Slot
                PlayerInventoryManager.weaponHotbar.LockSlots(this, true);
            }
        }

        // Play Animation
        main.Animator.Play(dashDir == main.Dir ? "Dash_Forward" : "Dash_Backward");
    }
    public override void OnFixedUpdate()
    {
        // Timer
        dashTime_Cur += Time.fixedDeltaTime;
        if (dashTime_Cur >= dashTime)
        {
            DashDone = true;
            return;
        }

        // Dash
        main.RB2D.velocity = new Vector2(dashDir * (dashDist / dashTime), 0);

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.Dashing);

        // Trail Effect
        if (dashTime_Cur >= dashTime * (curTrailCount / trailCount))
        {
            curTrailCount++;
            main.bodySpriteRenderer.SpawnTrail(spriteTrailObject, trailDuration, main.bodySpriteRenderer.transform);
        }
    }
    #endregion
}
