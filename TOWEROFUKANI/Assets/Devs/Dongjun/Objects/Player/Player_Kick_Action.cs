using UnityEngine;

public class Player_Kick_Action : CLA_ActionBase<Player>
{
    #region Var: Inspector
    [Header("Kick")]
    [SerializeField] private Transform dirTarget;
    [SerializeField] private float power;
    [SerializeField] private BoxCollider2D detectBox;
    [SerializeField] private LayerMask detectMask;

    [Header("Movement")]
    [SerializeField] private Vector2 velPercent = new Vector2(0.3f, 0.3f);
    [SerializeField] private float xVelDrag = 0.8f;

    [Header("Animation Duration")]
    [SerializeField] private float duration;

    [Header("Sprite Renderer")]
    [SerializeField] private SpriteRenderer bodySpriteRenderer;
    #endregion

    #region Var: Properties
    public bool IsKicking { get; private set; } = false;
    #endregion

    #region Var: Components
    private Animator animator;
    private Rigidbody2D rb2D;
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
        IsKicking = true;
        rb2D.velocity *= velPercent;

        Anim_Logic.SetAnimSpeed(animator, duration);
        Animation();
    }
    public override void OnExit()
    {
        Anim_Logic.ResetAnimSpeed(animator);
    }
    public override void OnLateUpdate()
    {
        // Detect Ground
        main.groundDetectionData.DetectGround(true, rb2D, transform);

        // Gravity
        Gravity_Logic.ApplyGravity(rb2D, main.gravityData);

        // Reduce X Velocity
        rb2D.velocity -= new Vector2(rb2D.velocity.x * xVelDrag * Time.fixedDeltaTime * main.Dir, 0);
    }
    #endregion

    #region Method: Kick
    private void Kick()
    {
        Collider2D[] hits = 
            Physics2D.OverlapBoxAll(
                transform.position + new Vector3(detectBox.offset.x * (bodySpriteRenderer.flipX ? -1 : 1), detectBox.offset.y), 
                detectBox.size, 
                0f, 
                detectMask);

        if (hits.Length == 0) return;

        Rigidbody2D hitRB2D = null;

        // Get Nearest Corpse
        float dist = -1;
        for (int i = 0; i < hits.Length; i++)
        {
            Rigidbody2D curHitRB2D = hits[i].GetComponent<Rigidbody2D>();
            if (curHitRB2D is null)
                return;

            float curDist = Vector2.Distance(hits[i].transform.position, transform.position);
            if (dist == -1 || dist > curDist)
            {
                dist = curDist;
                hitRB2D = curHitRB2D;
            }
        }

        if (hitRB2D is null) return;

        // Kick
        Vector2 kickDir = (dirTarget.position - transform.position).normalized;
        hitRB2D.velocity = new Vector2(kickDir.x * (bodySpriteRenderer.flipX ? -1 : 1), kickDir.y) * power;
    }
    #endregion

    #region Method: Animation
    private void Animation()
    {
        animator.Play("Player_Kick", 0, 0f);
    }
    #endregion

    #region Method: Anim Event
    private void OnKickAnim()
    {
        Kick();
    }
    private void OnKickFinish()
    {
        IsKicking = false;
    }
    #endregion
}
