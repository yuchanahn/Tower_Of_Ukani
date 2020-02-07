using UnityEngine;
using Dongjun.Helper;

public class Player_Kick : SSM_State_wMain<Player>
{
    #region Var: Inspector
    [Header("Walk")]
    [SerializeField] private PlayerWalkData walkData;

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
    [SerializeField] private SpriteRenderer kickEffectSpriteRenderer;
    #endregion

    #region Var: Attack Data
    private AttackData attackData;
    #endregion

    #region Var: Properties
    public bool IsKicking { get; private set; } = false;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        attackData = new AttackData(1);
    }
    #endregion

    #region Method: SSM
    public override void OnEnter()
    {
        IsKicking = true;

        main.rb2D.velocity = main.rb2D.velocity.Change(y: main.rb2D.velocity.y * yVelPercent);

        // Play Animation
        main.animator.SetDuration(duration);
        main.animator.Play("Kick", 0, 0f);

        // Flip Sprite
        kickEffectSpriteRenderer.flipX = main.bodySpriteRenderer.flipX;
    }
    public override void OnExit()
    {
        // Animation
        main.animator.ResetSpeed();
    }
    public override void OnFixedUpdate()
    {
        // Detect Ground
        main.groundDetectionData.DetectGround(true, main.rb2D, transform);

        // Gravity
        Gravity_Logic.ApplyGravity(main.rb2D, 
            main.groundDetectionData.isGrounded ? GravityData.Zero : 
            main.gravityData);

        // Walk
        walkData.Walk(PlayerInputManager.Inst.Input_WalkDir, main.rb2D, false);
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
