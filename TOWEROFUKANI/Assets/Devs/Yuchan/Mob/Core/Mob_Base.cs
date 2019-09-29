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

    GroundInfo mCurGroundInfo = new GroundInfo();

    protected Rigidbody2D m_rb;
    protected Animator m_ani;

    protected bool m_bGrounded = false;
    protected bool m_bJumping = false;
    protected bool m_bJumpStart = false;
    protected bool m_bFallStart = false;

    float t = 0;
    float rt;


    float VelX => m_MoveData.Speed * m_MoveData.Dir;
    float VelY => m_rb.velocity.y;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_ani = GetComponent<Animator>();
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



    private void Animation()
    {

    }
    public void OnMoveRandom()
    {
        rt = m_MoveData.MoveT.Get;
        m_MoveData.State = MobMoveData.eState.Move;
        m_MoveData.Dir = ARandom.Dir;
    }
    public void OnIdleRandom()
    {
        rt = m_MoveData.IdleT.Get;
        m_MoveData.State = MobMoveData.eState.Idle;
        m_MoveData.Dir = 0;
    }

    public bool MoveRandom()
    {
        t += Time.deltaTime;
        if (m_MoveData.State == MobMoveData.eState.Move && rt < t)
        {
            t = 0;
            OnIdleRandom();
        }
        return true;
    }

    public bool IdleRandom()
    {
        if (m_MoveData.State == MobMoveData.eState.Idle && rt < t)
        {
            t = 0;
            OnMoveRandom();
        }
        return true;
    }

    private void OnDestroy()
    {
    }

    public bool Idle() => true;


    public bool DetectCliff() => true;

    public bool Follow() => true;
    public bool Attack() => true;
    private void AttackEnd() { }


    public bool Hurting() => true;
    private void HurtEnd() { }




    #region Interface: IHurt
    public virtual void OnHurt()
    {

    }
    public virtual void OnDead()
    {
        Destroy(gameObject);
        CorpseMgr.CreateDeadbody(transform, m_compseData);
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