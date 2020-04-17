using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_Player_Kick : OBB_Player_State
{
    #region Var: Inspector
    [Header("Kick")]
    [SerializeField] private Transform kickObject;
    [SerializeField] private BoxCollider2D kickArea;
    [SerializeField] private Transform dirTarget;
    [SerializeField] private float kickPower;
    [SerializeField] private LayerMask blockMask;
    [SerializeField] private LayerMask mobMask;

    [Header("Movement")]
    [SerializeField] private float yVelPercent = 0.3f;

    [Header("Animation Duration")]
    [SerializeField] private float duration;
    #endregion

    #region Var: Attack Data
    private AttackData attackData;
    #endregion

    #region Var: Status
    private PlayerStatus_Slow status_Slow;
    #endregion

    #region Prop: 
    public bool KickDone
    { get; private set; } = false;
    #endregion

    #region Method: Unity
    private void Start()
    {
        attackData = new AttackData(1);
        status_Slow = new PlayerStatus_Slow(data.StatusID, gameObject, 50f);
    }
    #endregion

    #region Method: OBB
    public override void OnEnter()
    {
        // Reduce Y Velocity
        data.RB2D.velocity = data.RB2D.velocity.Change(y: data.RB2D.velocity.y * yVelPercent);

        // Flip Kick Object
        Flip_Logic.FlipXTo(data.Dir, kickObject);

        // Status Effect
        PlayerStatus.AddEffect(status_Slow);

        // Player State
        data.CanChangeDir = false;

        // Play Animation
        data.Animator.SetDuration(duration);
        data.Animator.Play("Kick", 0, 0f);
    }
    public override void OnExit()
    {
        KickDone = false;

        // Status Effect
        PlayerStatus.RemoveEffect(status_Slow);

        // Player State
        data.CanChangeDir = true;

        // Animation
        data.Animator.ResetSpeed();
    }
    public override void OnFixedUpdate()
    {
        // Detect Ground
        data.groundDetectionData.DetectGround(true, data.RB2D, transform);

        // Walk
        PlayerStats.Inst.walkData.Walk(PlayerInputManager.Inst.Input_WalkDir, data.RB2D, false);

        // Follow Moving Platform
        data.groundDetectionData.FollowMovingPlatform(data.RB2D);

        // Gravity
        Gravity_Logic.ApplyGravity(data.RB2D,
            data.groundDetectionData.isGrounded ? GravityData.Zero :
            data.gravityData);
    }
    #endregion

    #region Method: Kick
    private void KickBlock()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            kickArea.transform.position,
            kickArea.size * kickArea.transform.lossyScale,
            0f,
            blockMask);

        if (hits.Length == 0)
            return;

        Rigidbody2D hitRB2D = hits.GetClosest<Rigidbody2D>(transform);
        if (hitRB2D == null)
            return;

        // Kick Block
        hitRB2D.velocity = (dirTarget.position - transform.position).normalized * kickPower;
    }
    private void KickMob()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            kickArea.transform.position,
            kickArea.size * kickArea.transform.lossyScale,
            0f,
            mobMask);

        if (hits.Length == 0)
            return;

        // Damage Mob
        PlayerStats.Inst.DealDamage(attackData, hits.GetClosest(transform));
    }
    #endregion

    #region Method: Anim Event
    private void AnimEvent_Kick()
    {
        KickBlock();
        KickMob();
    }
    private void AnimEvent_KickEnd()
    {
        KickDone = true;
    }
    #endregion
}
