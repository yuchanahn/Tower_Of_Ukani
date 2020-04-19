using System.Collections;
using System.Collections.Generic;
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
    private float dashTime_Cur = 0;
    private int dashDir = 0;
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
    }
    #endregion

    #region Method: OBB
    public override void OnEnter()
    {
        // Init Value
        dashDir = PlayerInputManager.Inst.Input_DashDir;

        // Reset Collider Size
        data.groundDetectionData.Reset_IWSolid_ColSize();

        // Status Effect
        PlayerStatus.AddEffect(status_IgnoreDamage);

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.DashStart);
    }
    public override void OnExit()
    {
        // Reset Value
        DashDone = false;
        dashDir = 0;
        dashTime_Cur = 0;
        curTrailCount = 0;

        // Reset Vel
        data.RB2D.velocity = new Vector2(0, 0);

        // Status Effect
        PlayerStatus.RemoveEffect(status_IgnoreDamage);

        // Trigger Item Effect
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
        data.RB2D.velocity = new Vector2(dashDir * (dashDist / dashTime), 0);

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.Dashing);

        // Trail Effect
        if (dashTime_Cur >= dashTime * (curTrailCount / trailCount))
        {
            curTrailCount++;
            data.bodySpriteRenderer.SpawnTrail(spriteTrailObject, trailDuration, data.bodySpriteRenderer.transform);
        }
    }
    #endregion
}
