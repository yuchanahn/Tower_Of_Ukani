using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Base : MonoBehaviour, IHurt, ICanDetectGround
{
    #region Var: Inspector

    [Header("Collision")]
    [SerializeField]
    private BoxCollider2D m_OneWayCollider;


    [SerializeField] MobMoveData m_MoveData;
    [SerializeField] FollowData m_followData;
    [SerializeField] CorpseData m_compseData;
    [SerializeField] JumpData m_jumpData;
    [SerializeField] GravityData m_gravityData;
    [SerializeField] GroundDetectionData m_groundDetectionData;
    [SerializeField] CliffDetectionData m_cliffDetectionData;

    #endregion

    #region Component
    protected Rigidbody2D m_rb;
    protected Animator m_ani;
    #endregion

    GroundInfo mCurGroundInfo = new GroundInfo();

    protected bool m_bGrounded = false;
    protected bool m_bJumping = false;
    protected bool m_bJumpStart = false;
    protected bool m_bFallStart = false;




    float VelX => m_MoveData.Speed * m_MoveData.Dir;
    float VelY => m_rb.velocity.y;


    #region Method:
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_ani = GetComponent<Animator>();
        if (m_MoveData.State == MobMoveData.eState.Move) OnMoveRandom();
        if (m_MoveData.State == MobMoveData.eState.Idle) OnIdleRandom();
    }
    private void Update()
    {
        m_groundDetectionData.Size = m_OneWayCollider.size * transform.localScale;
    }
    private void FixedUpdate()
    {
        GroundDetection_Logic.DetectGround(!m_bJumping, m_rb, transform, m_groundDetectionData, ref m_bGrounded, ref mCurGroundInfo);
        GroundDetection_Logic.ExecuteOnGroundMethod(this, m_bGrounded, ref m_groundDetectionData);

        m_rb.velocity = new Vector2(VelX, VelY);

        GroundDetection_Logic.FallThrough(ref m_bFallStart, m_bGrounded, m_rb, transform, m_OneWayCollider, m_groundDetectionData);

        Jump_Logic.Jump(ref m_bJumpStart, ref m_bJumping, ref m_jumpData, m_rb, transform);

        Gravity_Logic.ApplyGravity(m_rb, m_bGrounded ? new GravityData(false, 0, 0) : !m_bJumping ? m_gravityData : new GravityData(true, m_jumpData.jumpGravity, 0));

        Animation();
    }
    #endregion

    private void Animation()
    {

    }

    #region Method: Move
    public void OnMoveRandom()
    {
        ATimer.Set(this, m_MoveData.MoveT.Get, OnIdleRandom);
        m_MoveData.State = MobMoveData.eState.Move;
        m_MoveData.Dir = ARandom.Dir;
    }
    public void OnIdleRandom()
    {
        ATimer.Set(this, m_MoveData.IdleT.Get, OnMoveRandom);
        m_MoveData.State = MobMoveData.eState.Idle;
        m_MoveData.Dir = 0;
    }
    public bool MoveRandom()
    {
        ATimer.Tick(this);
        return true;
    }
    public bool IdleRandom()
    {
        return true;
    }
    #endregion

    public bool DetectCliff() => true;

    public bool Follow() => true;
    public bool Attack() => true;
    private void AttackEnd() { }

    public virtual void OnHurt() { }
    public bool Hurting() => true;
    private void HurtEnd() { }




    #region Interface: IHurt

    public virtual void OnDead()
    {
        Destroy(gameObject);
        //CorpseMgr.CreateDeadbody(transform, m_compseData);
    }
    #endregion

    #region Interface: ICanDetectGround
    virtual public void OnGroundEnter()
    {
        Jump_Logic.ResetJump(ref m_jumpData);
    }
    virtual public void OnGroundStay()
    {
    }
    virtual public void OnGroundExit()
    {
    }
    #endregion
}