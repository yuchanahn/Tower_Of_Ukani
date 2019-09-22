using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Base : MonoBehaviour, IHurt, ICanDetectGround
{
    [SerializeField] private int mSpeed;
    [SerializeField] private float mAttackRange;
    [SerializeField] private float mFollowRange;

    [SerializeField] private JumpData mJumpData;
    [SerializeField] private GravityData mGravityData;
    [SerializeField] private BoxCollider2D mOneWayCollider;
    [SerializeField] private GroundDetectionData mGroundDetectionData;

    private int mCurDir;

    protected float mAddSpeed = 0;

    protected bool mbGrounded;
    protected bool mbJumping;
    protected bool mbHurt;
    protected bool mbKeepHurt;
    protected bool mbKeepAttack;
    protected bool mbAttackEnd = true;
    protected bool mbJumpPress = false;

    private GroundInfo curGroundInfo = new GroundInfo();

    private bool onGroundEnter = false;
    private bool onGroundExit = false;

    protected Rigidbody2D mRb2D;
    protected Animator mAnimator;

    protected float PlayerDis => (GM.PlayerPos - transform.position).magnitude;

    public int RandomDir => Random.Range(0, 2) == 0 ? -1 : 1;
    public bool IsHurt { get => mbHurt; set => mbHurt = value; }

    public bool InFollowRange => PlayerDis < mFollowRange;
    public bool InAttackRange => PlayerDis < mAttackRange;
    public int CurDir { get => mCurDir; set { mCurDir = value; if(mCurDir != 0) transform.eulerAngles = new Vector2(0, mCurDir == -1 ? 0 : 180); } }
    public bool IsKeepAttack => mbKeepAttack;

    public bool StartAttacking { get { return StartAttacking; } internal set { if (value) { OnAttack(); } } }

    private void Awake()
    {
        mRb2D = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (mbHurt)
        {
            mAnimator.Play(gameObject.name + "_Hit");
            mbHurt = false;
            mbKeepHurt = true;
        }


        mGroundDetectionData.size = mOneWayCollider.size * transform.localScale;
    }

    private void FixedUpdate()
    {
        if (!mbAttackEnd)
        {
            mAnimator.Play(gameObject.name + "_Attack");
        }
        else if (mbGrounded)
        {
            if (CurDir != 0) mAnimator.Play(gameObject.name + "_Walk");
            else mAnimator.Play(gameObject.name + "_Idle");
        }
        else
        {
            if (!mbJumping) mAnimator.Play(gameObject.name + "_AirborneDown");
            else mAnimator.Play(gameObject.name + "_AirborneUp");
        }
        
        // Detect Ground
        GroundDetection_Logic.DetectGround(!mbJumping, mRb2D, transform, mGroundDetectionData, ref mbGrounded, ref curGroundInfo);
        GroundDetection_Logic.ExecuteOnGroundMethod(this, mbGrounded, ref onGroundEnter, ref onGroundExit);


        mRb2D.velocity = new Vector2((mSpeed+mAddSpeed) * CurDir, mRb2D.velocity.y);
        Jump_Logic.Jump(ref mbJumpPress, ref mbJumping, ref mJumpData, mRb2D, transform);
        Gravity_Logic.ApplyGravity(mRb2D, mbGrounded ? new GravityData(0, 0) : !mbJumping ? mGravityData : new GravityData(mJumpData.jumpGravity, 0));
    }
    virtual public void OnGroundEnter()
    {
        // Reset Jump
        Jump_Logic.ResetJump(ref mbJumping, ref mJumpData);
    }
    virtual public void OnGroundStay()
    {
    }
    virtual public void OnGroundExit()
    {
    }
    protected void Jump()
    {
        mbJumpPress = true;
    }

    private void HurtEnd() => mbKeepHurt = false;


    virtual protected void OnAttack()
    {
        CurDir = GM.PlayerPos.x > transform.position.x ? 1 : -1;
        CurDir = 0;
        mbAttackEnd = false;
    }
    private void AttackEnd() { mbAttackEnd = true; }


    public void OnHurt()
    {
        mbHurt = true;
    }

    public bool FollowPlayer()
    {
        CurDir = GM.PlayerPos.x > transform.position.x ? 1 : -1;
        return true;
    }

    public bool Attack()
    {
        return !mbAttackEnd;
    }
}