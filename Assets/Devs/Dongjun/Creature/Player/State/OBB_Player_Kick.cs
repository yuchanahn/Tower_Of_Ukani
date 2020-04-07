using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_Player_Kick : OBB_Player_State
{
    #region Var: Inspector
    [Header("Kick")]
    [SerializeField] private Transform kickObject;
    [SerializeField] private float power;
    [SerializeField] private Transform dirTarget;
    [SerializeField] private Transform detectPoint;
    [SerializeField] private Vector2 detectSize;
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(detectPoint.position, detectSize);
    }
    private void Start()
    {
        attackData = new AttackData(1);

        status_Slow = new PlayerStatus_Slow(data.StatusID, gameObject, 50f);
    }
    #endregion

    #region Method: OBB
    public override void OnEnter()
    {
        data.RB2D.velocity = data.RB2D.velocity.Change(y: data.RB2D.velocity.y * yVelPercent);

        // Status Effect
        PlayerStatus.AddEffect(status_Slow);

        // Look Dir
        Flip_Logic.FlipXTo(data.Dir, kickObject);

        // Play Animation
        data.Animator.SetDuration(duration);
        data.Animator.Play("Kick", 0, 0f);

        // Player
        data.CanChangeDir = false;
    }
    public override void OnExit()
    {
        KickDone = false;

        // Status Effect
        PlayerStatus.RemoveEffect(status_Slow);

        // Animation
        data.Animator.ResetSpeed();

        // Player
        data.CanChangeDir = true;
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
        Collider2D[] blockHits = Physics2D.OverlapBoxAll(detectPoint.position, detectSize, 0f, blockMask);

        if (blockHits.Length == 0)
            return;

        Rigidbody2D hitRB2D = blockHits.GetClosest<Rigidbody2D>(transform);
        if (hitRB2D == null)
            return;

        // Kick
        hitRB2D.velocity = (dirTarget.position - transform.position).normalized * power;
    }
    private void KickMob()
    {
        Collider2D[] mobHits = Physics2D.OverlapBoxAll(detectPoint.position, detectSize, 0f, mobMask);

        if (mobHits.Length == 0)
            return;

        PlayerStats.Inst.DealDamage(attackData, mobHits.GetClosest(transform));
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
