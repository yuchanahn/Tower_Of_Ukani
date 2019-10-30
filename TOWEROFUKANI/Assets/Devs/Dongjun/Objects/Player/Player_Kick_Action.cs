using UnityEngine;

public class Player_Kick_Action : CLA_Action
{
    #region Var: Inspector
    [Space, Header("Kick")]
    [SerializeField] private Transform dirTarget;
    [SerializeField] private float power;
    [SerializeField] private BoxCollider2D detectBox;
    [SerializeField] private LayerMask detectMask;

    [Space, Header("Animation Duration")]
    [SerializeField] private TimerData durationData;

    [Space, Header("Sprite Renderer")]
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
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        durationData.Init(gameObject, OnEnd: Finish);
    }
    #endregion

    #region Method: CLA_Action
    public override void OnEnter()
    {
        durationData.SetActive(true);
        durationData.Restart();

        rb2D.velocity = new Vector2(0, 0);
        IsKicking = true;

        Anim_Logic.SetAnimSpeed(animator, durationData.EndTime);
        Animation();
    }
    public override void OnExit()
    {
        durationData.SetActive(false);
        durationData.ToZero();

        Anim_Logic.ResetAnimSpeed(animator);
    }
    #endregion

    #region Method: Kick
    private void Finish()
    {
        IsKicking = false;
    }
    private void Kick()
    {
        Collider2D[] hits = 
            Physics2D.OverlapBoxAll(
                transform.position + new Vector3(detectBox.offset.x * (bodySpriteRenderer.flipX ? -1 : 1), detectBox.offset.y), 
                detectBox.size, 
                0f, 
                detectMask);

        if (hits is null)
            return;

        float dist = -1;
        Rigidbody2D hitRB2D = null;

        // Get Nearest Corpse
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

        if (hitRB2D is null)
            return;

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
    #endregion
}
