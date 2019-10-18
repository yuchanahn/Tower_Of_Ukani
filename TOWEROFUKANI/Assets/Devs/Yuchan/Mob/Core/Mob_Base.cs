using Shiroi.Pathfinding2D.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eMobAniST
{
    Attack_Pre,
    Attack_Post,
    Hit,
    Walk,
    Idle,
    AirborneDown,
    AirborneUp,
    Last,
}

[Serializable]
public struct AniSpeedData
{
    public eMobAniST ST;
    public float t;
}

public class Mob_Base : MonoBehaviour, IHurt, ICanDetectGround
{
    #region Var: Inspector

    [Header("Collision")]
    [SerializeField] BoxCollider2D m_OneWayCollider;

    [SerializeField] protected eMobAniST m_CurAniST;

    [Header("Attack")]
    [SerializeField] float m_AttackRange;

    [SerializeField] AniSpeedData[] m_AniSpeedData;
    [SerializeField] MobMoveData m_MoveData;
    [SerializeField] FollowData m_followData;
    [SerializeField] CorpseData m_compseData;
    [SerializeField] protected JumpData m_jumpData;
    [SerializeField] GravityData m_gravityData;
    [SerializeField] GroundDetectionData m_groundDetectionData;

    #endregion

    #region Var: Component
    protected Rigidbody2D m_rb;
    protected Animator m_ani;
    #endregion

    #region Var:
    Dictionary<eMobAniST, (string, float)> m_Ani = new Dictionary<eMobAniST, (string, float)>();

    protected bool m_bJumpStart = false;
    protected bool m_bFallStart = false;
    protected bool m_bHurting = false;
    protected bool m_bAniStart = false;
    protected bool m_bYMoveCoolTime = false;
    protected bool m_bMoveAble = true;
    protected bool m_bAttacking = false;
    protected bool m_bFollowJump = false;


    protected float m_jumpHeight;


    protected int m_bPrevDir = 0;
    #endregion

    #region Var: Properties
    public Vector2 JumpVel;
    public virtual float VelX =>
     m_bAttacking ? 0 :
     CanAttack ? 0 :
     CanFollow ?  m_MoveData.Speed * m_MoveData.Dir :
     m_bHurting || !m_groundDetectionData.isGrounded ? 0
    : !CliffDetect_Logic.CanFall(m_MoveData.FallHeight, transform, m_MoveData.Dir * m_groundDetectionData.Size.x, m_groundDetectionData.GroundLayers) ? 0
    : m_MoveData.Speed * m_MoveData.Dir;

    float VelY => m_rb.velocity.y;
    protected float WalkSpeed => m_MoveData.Speed * m_MoveData.Dir;
    protected int Dir { set { m_MoveData.Dir = value; } get { return m_MoveData.Dir; } }





    public virtual bool CanFollow => ((GM.PlayerPos - transform.position).magnitude < m_followData.dist);

    public virtual bool CanAttack => m_bAttacking ? true : !m_groundDetectionData.isGrounded ? false : m_bAttacking = ((GM.PlayerPos - transform.position).magnitude < m_AttackRange);
            
    #endregion

    #region Method:
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_ani = GetComponent<Animator>();
        if (m_MoveData.State == MobMoveData.eState.Move) OnMoveRandom();
        if (m_MoveData.State == MobMoveData.eState.Idle) OnIdleRandom();

        for (int i = 0; i < (int)eMobAniST.Last; i++)
            m_Ani[(eMobAniST)i] = ($"{gameObject.name}_{((eMobAniST)i).ToString()}", 0);
        foreach (var i in m_AniSpeedData)
            m_Ani[i.ST] = ($"{gameObject.name}_{i.ST.ToString()}", i.t);

        m_groundDetectionData.Size = m_OneWayCollider.size;
        m_atkAct[eAtkST.post] = AttackPost;
        m_atkAct[eAtkST.pre] = AttackPre;
        m_jumpHeight = m_jumpData.height;
    }
    private void FixedUpdate()
    {
        m_groundDetectionData.DetectGround(!m_jumpData.isJumping, m_rb, transform);
        m_groundDetectionData.ExecuteOnGroundMethod(this);

        m_rb.velocity = new Vector2(m_bFollowJump ? JumpVel.x : VelX, VelY);

        m_groundDetectionData.FallThrough(ref m_bFallStart, m_rb, transform, m_OneWayCollider);

        
        m_jumpData.Jump(ref m_bJumpStart, m_rb, transform);

        Gravity_Logic.ApplyGravity(m_rb, m_groundDetectionData.isGrounded ? new GravityData(false, 0, 0) : !m_jumpData.isJumping ? m_gravityData : new GravityData(true, m_jumpData.jumpGravity, 0));

        Animation();
        Anim_Logic.SetAnimSpeed(m_ani, m_Ani[m_CurAniST].Item2);
        if (m_bAniStart) { m_ani.Play(m_Ani[m_CurAniST].Item1, 0, 0); m_bAniStart = false; }
        else m_ani.Play(m_Ani[m_CurAniST].Item1);
    }
    private void OnDestroy()
    {
        ATimer.Pop("JumpFall" + GetInstanceID());
    }
    #endregion
    
    void Animation()
    {
        m_CurAniST = m_bHurting || m_bAttacking ? m_CurAniST : m_groundDetectionData.isGrounded ? Dir != 0 ? eMobAniST.Walk : eMobAniST.Idle : VelY > 0 ? eMobAniST.AirborneUp : eMobAniST.AirborneDown;
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
    }
    public bool MoveRandom()
    {
        if (!m_groundDetectionData.isGrounded) return false;
        m_MoveData.State = MobMoveData.eState.Move;
        m_CurAniST = eMobAniST.Walk;
        ATimer.Tick(this);
        if (!m_bYMoveCoolTime && !m_bJumpStart)
        {
            m_bYMoveCoolTime = ARandom.Get(70) ? true : ARandom.Get(50) ? Jump() : Fall();
            m_bYMoveCoolTime = true;
            ATimer.Set("JumpFall" + GetInstanceID(), m_MoveData.CoolTime, () => { m_bYMoveCoolTime = false; });
        }
        return true;
    }
    public bool IdleRandom()
    {
        if (!m_groundDetectionData.isGrounded) return false;
        if (m_MoveData.State == MobMoveData.eState.Idle)
        {
            m_CurAniST = eMobAniST.Idle;
            m_MoveData.Dir = 0;
        }
        return true;
    }
    public bool Falling()
    {
        m_CurAniST = m_jumpData.isJumping ? eMobAniST.AirborneUp : eMobAniST.AirborneDown;
        return true;
    }
    public bool Jump()
    {
        if (m_bJumpStart = MobYMoveDetect_Logic.UP(transform.position, ref m_jumpData, ref m_MoveData))
            m_CurAniST = eMobAniST.AirborneUp;

        return m_bJumpStart;
    }
    public bool Fall()
    {
        if (m_bFallStart = MobYMoveDetect_Logic.Down(transform.position, m_groundDetectionData.Size, ref m_MoveData))
            m_CurAniST = eMobAniST.AirborneDown;

        return m_bFallStart;
    }

    #endregion

    // TODO : 

    public int FollowJump(Vector2 nom)
    {
        JumpVel = nom;
        m_bJumpStart = true;
        m_bFollowJump = true;
        m_jumpData.height = JumpVel.y;
        return nom.x < 0 ? -1 : 1;
    }

    public bool Follow()
    {
        if (m_bFollowJump) return false;
        var pa = PathFinder.Inst.FindPath(transform.position.GetGorundOfBottomPos(m_groundDetectionData.Size, m_followData.CantMoveGround), GM.PlayerPos.GetGorundOfBottomPos(GM.PlayerSize, m_followData.CantMoveGround));
        if (!pa.bFollow) return false;
        Dir = pa.bJump ? FollowJump(pa.nomal) : pa.nomal.x < 0 ? -1 : 1;
        

        return true;
    }

    enum eAtkST
    {
        pre,
        post
    }
    eAtkST atkST = eAtkST.pre;
    Dictionary<eAtkST, Func<bool>> m_atkAct = new Dictionary<eAtkST, Func<bool>>();
    public bool Attack() => m_atkAct[atkST]();
    public bool AttackPre()
    {
        m_CurAniST = eMobAniST.Attack_Pre;
        return m_bAttacking;
    }
    private void AttackPreEnd()
    {
        atkST = eAtkST.post;
    }
    public bool AttackPost()
    {
        m_CurAniST = eMobAniST.Attack_Post;
        return m_bAttacking;
    }
    private void AttackPostEnd()
    {
        m_bAttacking = false;
        atkST = eAtkST.pre;
        OnAttackEnd();
    }

    public virtual void OnAttackEnd()
    {
    }

    public virtual void OnHurt(/* 총알 위치 */)
    {
        if (!m_bHurting)
        {
            m_bPrevDir = m_MoveData.SprDir;
            m_MoveData.SprDir *= (Mathf.Sign(GM.PlayerPos.x - transform.position.x) == m_bPrevDir) ? -1 : 1;
        }
        m_bHurting = true;
        m_bAniStart = true;

        m_CurAniST = eMobAniST.Hit;
    }
    public bool Hurting() => m_bHurting;
    private void HurtEnd()
    {
        m_bAttacking = false;
        atkST = eAtkST.pre;
        m_bHurting = false;
        m_MoveData.SprDir = m_bPrevDir;
    }

    #region Interface: IHurt

    public virtual void OnDead()
    {
        Destroy(gameObject);
        CorpseMgr.CreateCorpseOrNull(transform, m_compseData);
    }
    #endregion

    #region Interface: ICanDetectGround
    virtual public void OnGroundEnter()
    {
        Jump_Logic.ResetJumpCount(ref m_jumpData);
        m_bFollowJump = false;
        m_CurAniST = eMobAniST.Idle;
    }
    virtual public void OnGroundStay()
    {
    }
    virtual public void OnGroundExit()
    {
    }
    #endregion
}