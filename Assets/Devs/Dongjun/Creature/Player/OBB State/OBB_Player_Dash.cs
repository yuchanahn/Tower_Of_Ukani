using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OBB_Player_Dash : OBB_Player_State
{
    #region Var: Inspector
    [Header("Dash")]
    [SerializeField] private float dashDist;
    [SerializeField] private float dashTime;
    [SerializeField] private float staminaUsage = 1;

    [Header("Effect")]
    [SerializeField] private SpriteTrailObject spriteTrailObject;
    [SerializeField] private float trailDuration = 0.15f;
    [SerializeField] private float trailCount = 4;
    #endregion

    #region Var: Dash
    private readonly TimerData dashTimer = new TimerData();
    private int dashDir = 0;
    private float curDist = 0;
    private float curSpeed = 0;
    private float curTime = 0;
    private bool isUsingMelee = false;
    #endregion

    #region Var: Status
    private PlayerStatus_IgnoreDamage status_IgnoreDamage;
    #endregion

    #region Var: Visual Effect
    int curTrailCount = 0;
    #endregion

    #region Prop: 
    public float StaminaUsage => staminaUsage;
    public bool DashDone { get; private set; } = false;
    #endregion

    #region Method: Unity
    private void Start()
    {
        // Init Status
        status_IgnoreDamage = new PlayerStatus_IgnoreDamage(data.StatusID, gameObject);

        // Init Timer
        dashTimer.EndTime = dashTime;
        dashTimer
            .SetTick(gameObject, TickMode.FixedUpdate)
            .SetActive(false)
            .SetAction(
                onTick: () =>
                {
                    float distLeft = dashDist - curDist;
                    float timeLeft = dashTime - curTime;
                    float clampedDeltaTime = Mathf.Min(Time.fixedDeltaTime, timeLeft);

                    curSpeed = distLeft / timeLeft;
                    curTime = Mathf.Min(dashTime, curTime + clampedDeltaTime);
                    curDist = Mathf.Min(dashDist, curDist + (curSpeed * clampedDeltaTime));

                    data.RB2D.velocity = Vector2.right * dashDir * (curSpeed * clampedDeltaTime / Time.fixedDeltaTime);

                    // Trigger Event
                    PlayerActionEventManager.Trigger(PlayerActions.Dashing);

                    // Trail Effect
                    if (curTime >= dashTime * (curTrailCount / trailCount))
                    {
                        curTrailCount++;
                        data.bodySpriteRenderer.SpawnTrail(spriteTrailObject, trailDuration, data.bodySpriteRenderer.transform);
                    }
                },
                onEnd: () =>
                {
                    DashDone = true;
                });
    }
    #endregion

    #region Method: OBB
    public override void OnEnter()
    {
        // Init Value
        dashDir = PlayerInputManager.Inst.Input_DashDir;
        dashTimer.SetActive(true);

        // Reset Collider Size
        //data.groundDetectionData.Reset_IWSolid_ColSize();

        // Reset Velocity
        data.RB2D.velocity = Vector2.zero;

        // Apply Status Effect
        PlayerStatus.AddEffect(status_IgnoreDamage);

        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.DashStart);
    }
    public override void OnExit()
    {
        // Reset Value
        dashTimer.SetActive(false).Reset();
        dashDir = 0;
        curDist = 0;
        curSpeed = 0;
        curTime = 0;
        curTrailCount = 0;
        DashDone = false;

        // Reset Velocity
        data.RB2D.velocity = Vector2.zero;

        // Remove Status Effect
        PlayerStatus.RemoveEffect(status_IgnoreDamage);

        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.DashEnd);

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
                data.bodySpriteRenderer.flipX = dashDir == 1 ? false : true;

                // Lock Slot
                PlayerInventoryManager.weaponHotbar.LockSlots(this, true);
            }
        }

        // Play Animation
        data.Animator.Play(dashDir == data.Dir ? "Dash_Forward" : "Dash_Backward");
    }
    #endregion
}
