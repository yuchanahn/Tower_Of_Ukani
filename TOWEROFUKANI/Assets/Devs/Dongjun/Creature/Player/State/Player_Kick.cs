﻿using UnityEngine;
using Dongjun.Helper;

public class Player_Kick : SSM_State_wMain<Player>
{
    #region Var: Inspector
    [Header("Kick")]
    [SerializeField] private Transform dirTarget;
    [SerializeField] private float power;
    [SerializeField] private BoxCollider2D detectBox;
    [SerializeField] private LayerMask blockMask;
    [SerializeField] private LayerMask mobMask;

    [Header("Movement")]
    [SerializeField] private float yVelPercent = 0.3f;

    [Header("Animation Duration")]
    [SerializeField] private float duration;

    [Header("Sprite Renderer")]
    [SerializeField] private SpriteRenderer effectSpriteRenderer;
    #endregion

    #region Var: Attack Data
    private AttackData attackData;
    #endregion

    #region Var: Status
    private PlayerStatus_Slow status_Slow;
    #endregion

    #region Var: Properties
    public bool IsKicking
    { get; private set; } = false;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        attackData = new AttackData(1);

        status_Slow = new PlayerStatus_Slow(GM.Player.StatusID, GM.Player.gameObject, 50f);
    }
    #endregion

    #region Method: SSM
    public override void OnEnter()
    {
        IsKicking = true;

        main.RB2D.velocity = main.RB2D.velocity.Change(y: main.RB2D.velocity.y * yVelPercent);

        // Status Effect
        PlayerStatus.AddEffect(status_Slow);

        // Flip Sprite
        effectSpriteRenderer.flipX = main.bodySpriteRenderer.flipX;

        // Play Animation
        main.Animator.SetDuration(duration);
        main.Animator.Play("Kick", 0, 0f);
    }
    public override void OnExit()
    {
        // Status Effect
        PlayerStatus.RemoveEffect(status_Slow);

        // Animation
        main.Animator.ResetSpeed();
    }
    public override void OnFixedUpdate()
    {
        // Detect Ground
        main.groundDetectionData.DetectGround(true, main.RB2D, transform);

        // Gravity
        Gravity_Logic.ApplyGravity(main.RB2D, 
            main.groundDetectionData.isGrounded ? GravityData.Zero : 
            main.gravityData);

        // Walk
        PlayerStats.Inst.walkData.Walk(PlayerInputManager.Inst.Input_WalkDir, main.RB2D, false);
    }
    #endregion

    #region Method: Kick
    private void KickBlock()
    {
        Collider2D[] blockHits = 
            Physics2D.OverlapBoxAll(
                transform.position + new Vector3(detectBox.offset.x * main.Dir, detectBox.offset.y), 
                detectBox.size, 
                0f, 
                blockMask);

        if (blockHits.Length == 0)
            return;

        Rigidbody2D hitRB2D = blockHits.GetClosest<Rigidbody2D>(transform);
        if (hitRB2D == null)
            return;

        // Kick
        Vector2 kickDir = (dirTarget.position - transform.position).normalized;
        hitRB2D.velocity = new Vector2(kickDir.x * main.Dir, kickDir.y) * power;
    }
    private void KickMob()
    {
        Collider2D[] mobHits =
            Physics2D.OverlapBoxAll(
                transform.position + new Vector3(detectBox.offset.x * main.Dir, detectBox.offset.y),
                detectBox.size,
                0f,
                mobMask);

        if (mobHits.Length == 0)
            return;

        PlayerStats.Inst.DealDamage(attackData, mobHits.GetClosest(transform));
    }
    #endregion

    #region Method: Anim Event
    private void OnKickAnim()
    {
        KickBlock();
        KickMob();
    }
    private void OnKickFinish()
    {
        IsKicking = false;
    }
    #endregion
}
