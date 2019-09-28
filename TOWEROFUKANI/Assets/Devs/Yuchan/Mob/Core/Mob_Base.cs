﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Base : MonoBehaviour, IHurt, ICanDetectGround
{
    #region Var: Inspector
    [SerializeField] private int mSpeed;
    [SerializeField] private float mAttackRange;
    [SerializeField] private float mFollowRange;

    [Header("Collision")]
    [SerializeField]
    private BoxCollider2D oneWayCollider;

    [Header("MoveAbleGroundLayers")]
    [SerializeField]
    private LayerMask MoveAbleGroundLayers;


    [SerializeField] private FollowData mFollowData;
    [SerializeField] private CorpseData mCompseData;
    [SerializeField] private JumpData mJumpData;
    [SerializeField] private GravityData mGravityData;
    [SerializeField] private BoxCollider2D mOneWayCollider;
    [SerializeField] private GroundDetectionData mGroundDetectionData;
    #endregion

    private int mCurDir;

    protected float mAddSpeed = 0;

    protected bool mbGrounded;
    protected bool mbJumping;
    protected bool mbHurt;
    protected bool mbFollowAble = true;
    protected bool mbAttackEnd = true;
    protected bool mbJumpPress = false;

    private GroundInfo mCurGroundInfo = new GroundInfo();

    private bool onGroundEnter = false;
    private bool onGroundExit = false;

    protected Rigidbody2D mRb2D;
    protected Animator mAnimator;

    #region Var: Properties
    protected float PlayerDis => (GM.PlayerPos - transform.position).magnitude;
    protected float PlayerDisX => Mathf.Abs(GM.PlayerPos.x - transform.position.x);
    protected float PlayerDisY => Mathf.Abs(GM.PlayerPos.y - transform.position.y);
    protected LayerMask GroundLayers => mGroundDetectionData.GroundLayers;
    protected Vector2 Size => (mOneWayCollider.size * transform.localScale.normalized);



    virtual public int RandomDir => IsKeepAttack ? CurDir : Random.Range(0, 2) == 0 ? -1 : 1;
    public bool IsHurt { get => mbHurt; set => mbHurt = value; }
    public bool IsOnCliff
    {
        get {
            if (!mbGrounded) return false;
            var hit = Physics2D.RaycastAll(transform.position, new Vector2(CurDir, -1), mOneWayCollider.size.x, GroundLayers);
            Debug.DrawRay(transform.position, new Vector2(CurDir, -1) * (mOneWayCollider.size.x), Color.red);

            if (hit.Length > 0) return false;
            return true;
        }
    }
        //=> !Physics2D.Raycast(transform.position, new Vector2(CurDir, -1), mOneWayCollider.size.x / 2);
    public bool InFollowRange => PlayerDis <= mFollowRange;
    public bool InAttackRange => mbGrounded && PlayerDis <= mAttackRange;
    public int CurDir { get => mCurDir; set { mCurDir = value; if(mCurDir != 0) transform.eulerAngles = new Vector2(0, mCurDir == -1 ? 0 : 180); } }
    public bool IsKeepAttack => !mbAttackEnd;


    public bool StartAttacking { get { return StartAttacking; } internal set { if (value) { OnAttack(); } } }
    #endregion

    #region Method: Unity
    private void Awake()
    {
        mRb2D = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        mGroundDetectionData.Size = mOneWayCollider.size * transform.localScale;
    }
    private void FixedUpdate()
    {
        Animation();

        // Detect Ground
        GroundDetection_Logic.DetectGround(!mbJumping, mRb2D, transform, mGroundDetectionData, ref mbGrounded, ref mCurGroundInfo);
        GroundDetection_Logic.ExecuteOnGroundMethod(this, mbGrounded, ref mGroundDetectionData);

        // Walk
        mRb2D.velocity = new Vector2((mSpeed+mAddSpeed) * CurDir, mRb2D.velocity.y);

        // Jump
        Jump_Logic.ResetJumpingState(ref mbJumping, ref mJumpData, mRb2D, transform);

        // Gravity
        Gravity_Logic.ApplyGravity(mRb2D, mbGrounded ? new GravityData(false, 0, 0) : !mbJumping ? mGravityData : new GravityData(true, mJumpData.jumpGravity, 0));
    }
    #endregion

    private void Animation()
    {
        if (IsKeepAttack)
        {
            mAnimator.Play(string.Concat(gameObject.name, "_Attack"));
        }
        else if (IsHurt)
        {
            mAnimator.Play(string.Concat(gameObject.name, "_Hit"));
        }
        else if (mbGrounded)
        {
            mAnimator.Play(string.Concat(gameObject.name, CurDir != 0 ? "_Walk" : "_Idle"));
        }
        else
        {
            mAnimator.Play(string.Concat(gameObject.name, mRb2D.velocity.y <= 0 ? "_AirborneDown" : "_AirborneUp"));
        }
    }

    protected int Jump()
    {
        Jump_Logic.Jump(ref mbJumping, ref mJumpData, mRb2D, transform);
        return 1;
    }
    protected int DownJump()
    {
        bool fall = true;
        GroundDetection_Logic.FallThrough(ref fall, mbGrounded, mRb2D, transform, oneWayCollider, mGroundDetectionData);
        return -1;
    }



    public bool FollowPlayer()
    {
        if(mbGrounded)
        switch (MobFollow_Logic.Follow(transform.position, mFollowData))
        {
            case MobFollow_Logic.eFollowState.Jump:
                Jump();
                break;
            case MobFollow_Logic.eFollowState.DownJump:
                DownJump();
                break;
            default:
                break;
        }
        CurDir = Mathf.Abs(GM.PlayerPos.x - transform.position.x) < 0.1f ? 0 : GM.PlayerPos.x > transform.position.x ? 1 : -1;
        return true;
    }

    virtual protected void OnAttack()
    {
        CurDir = GM.PlayerPos.x > transform.position.x ? 1 : -1;
        CurDir = 0;
        mbAttackEnd = false;
    }
    virtual protected void ResetAttack()
    {
        mbAttackEnd = true;
    }


    public bool Attack()
    {
        return !mbAttackEnd;
    }

    public void OnHurt()
    {
        mAnimator.Play(gameObject.name + "_Hit", 0, 0);
        ResetAttack();
        CurDir = 0;
        mbHurt = true;
    }

    public virtual void OnDead()
    {
        Destroy(gameObject);
        CorpseMgr.CreateDeadbody(transform ,mCompseData);
    }

    public void CliffCheck()
    {

    }

    #region AniEvent
    private void AttackEnd() => mbAttackEnd = true;
    private void HurtEnd() => mbHurt = false;
    #endregion

    #region Interface: ICanDetectGround
    virtual public void OnGroundEnter()
    {
        // Reset Jump
        Jump_Logic.ResetJump(ref mJumpData);
    }
    virtual public void OnGroundStay()
    {
    }
    virtual public void OnGroundExit()
    {
    }
    #endregion
}