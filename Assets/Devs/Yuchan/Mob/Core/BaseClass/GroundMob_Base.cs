using Dongjun.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




[Serializable]
public struct StatusEffectData
{
    public int AttackStop;
    public int MoveStop;
    public int HurtStop;
    public int UseStatusEffect;
    public eMobAniST StatusEffect;
    public int priority;
}

public abstract class GroundMob_Base : Mob_Base, ICanDetectGround
{
    #region Var: Inspector

    [Header("Collision")]
    [SerializeField] BoxCollider2D m_OneWayCollider;

    [SerializeField] protected eMobAniST m_CurAniST;

    [Header("Attack")]
    [SerializeField] float m_AttackRange;

    [Header("Data")]
    [SerializeField] MobMoveData m_MoveData;
    [SerializeField] FollowData m_followData;
    [SerializeField] protected JumpData m_jumpData;
    [SerializeField] GravityData m_gravityData;
    [SerializeField] GroundDetectionData m_groundDetectionData;


    [Header("Event")]
    public UnityEvent OnFollowing;
    public UnityEvent EndFollowing;

    #endregion

    #region Var: Component
    protected Rigidbody2D m_rb;
    protected Animator m_ani;
    protected StatusEffect_Object m_SEObj;
    #endregion

    #region Var:

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

    public virtual float VelX =>
     m_bAttacking ? 0 :
     CanAttack ? 0 :
     m_bHurting ? 0 :
     IsWallInForword ? IsOneWayInForword ? 0 : m_MoveData.Speed * m_MoveData.Dir :
     IsKeepFollowing ? m_MoveData.Speed * m_MoveData.Dir :
     IsFollowMax ? 0 :
     IsCliff ? 0 :
     CanFollow ?  m_MoveData.Speed * m_MoveData.Dir :
     
     m_bHurting || !m_groundDetectionData.isGrounded ? 0 :
     //: !CliffDetect_Logic.CanFall(m_MoveData.FallHeight, transform, m_MoveData.Dir * m_groundDetectionData.Size.x, m_groundDetectionData.GroundLayers) ? 0
     m_MoveData.Speed * m_MoveData.Dir;

    float VelY => m_rb.velocity.y;
    protected float WalkSpeed => m_MoveData.Speed * m_MoveData.Dir;
    protected int Dir { set { if(m_SEObj.ChangeDirAble) m_MoveData.Dir = value; } get { return m_MoveData.Dir; } }

    public int DirForPlayer => (GM.PlayerPos.x - transform.position.x) > 0 ? 1 : -1;
    public bool IsFollowMax { get; set; } = false;
    public bool IsKeepFollowing { get; set; } = false;

    public bool IsCliff => !CliffDetect_Logic.CanFall2(
        m_MoveData.FallHeight,
        transform.position,
        Dir,
        m_MoveData.Speed,
        m_groundDetectionData.Size,
        m_groundDetectionData.GroundLayers);

    public bool IsWallInForword => !CliffDetect_Logic.CanGo(
        transform.position,
        Dir,
        m_MoveData.Speed,
        m_groundDetectionData.Size,
        m_groundDetectionData.GroundLayers);

    public bool IsJumpHeightHitWall => !CliffDetect_Logic.CanGo2(
    transform.position,
    m_jumpHeight,
    Dir,
    m_MoveData.Speed,
    m_groundDetectionData.Size,
    m_followData.CantMoveGround, true);

    public bool IsOneWayInForword => !CliffDetect_Logic.CanGo(
    transform.position,
    Dir,
    m_MoveData.Speed,
    m_groundDetectionData.Size,
    m_followData.CantMoveGround);

    public virtual bool CanFollow =>
         !m_SEObj.FallowAble ? false :
         Hurting() ? false :
         ((GM.PlayerPos - transform.position).magnitude < m_followData.dist);

    public virtual bool CanAttack =>
        !m_SEObj.AttackAble ? false : 
        m_bAttacking ? true : 
        !m_groundDetectionData.isGrounded ? false :
        m_bAttacking = ((GM.PlayerPos - transform.position).magnitude < m_AttackRange);

    public bool IsAttacking => m_bAttacking;

    #endregion

    #region Method:
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_ani = GetComponent<Animator>();
        m_SEObj = GetComponent<StatusEffect_Object>();
        if (m_MoveData.State == MobMoveData.eState.Move) OnMoveRandom();
        if (m_MoveData.State == MobMoveData.eState.Idle) OnIdleRandom();

        Init_AniDuration(); // 애니 시간.

        m_groundDetectionData.Size = m_OneWayCollider.size;
        
        // Init Attack Ani?
        m_atkAct[eAtkST.post] = AttackPost;
        m_atkAct[eAtkST.pre] = AttackPre;

        m_jumpHeight = m_jumpData.height;
    }
    float CurveTime = 0f;
    private void FixedUpdate()
    {
        m_groundDetectionData.DetectGround(!m_jumpData.isJumping, m_rb, transform);
        m_groundDetectionData.ExecuteOnGroundMethod(this);
        var OverlapSlowSpeed = (CheckOverlapSlow(MobSize, new Vector2(m_MoveData.Dir, 0)) ? OverlapSlow : 1f);
        m_rb.velocity = new Vector2(m_bFollowJump && m_jumpData.isJumping ? VelX : VelX * m_SEObj.SpeedMult * OverlapSlowSpeed, VelY);
        if(m_SEObj.UseSEVel) 
            m_rb.velocity = m_SEObj.EffectDir2d * m_SEObj.SpeedMult;
        if (!m_SEObj.UseSEVelCurve) CurveTime = 0f;
        if (m_SEObj.UseSEVelCurve)
        {
            m_rb.velocity = m_SEObj.EffectDir2d * m_SEObj.VelCurve.Evaluate(CurveTime);
            CurveTime += Time.fixedDeltaTime;
        }
        m_groundDetectionData.FallThrough(ref m_bFallStart, m_rb, transform, m_OneWayCollider);
        
        
        m_jumpData.Jump(ref m_bJumpStart, m_rb, transform);


        Gravity_Logic.ApplyGravity(m_rb, m_groundDetectionData.isGrounded ? new GravityData(false, 0, 0) :
            m_bFollowJump ? m_gravityData :
            !m_jumpData.isJumping ? m_gravityData : new GravityData(true, m_jumpData.jumpGravity, 0));


        Animation();
        m_ani.SetDuration(m_Ani[m_CurAniST].Item2);
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
        EndFollowing.Invoke();
        if (m_SEObj.Ani != eMobAniST.Last)
        {
            m_CurAniST = m_SEObj.Ani;
            m_bHurting = false;
            return;
        }
        if (m_bHurting) return;

        if (!m_bAttacking)
        {
            if (CanFollow && Follow()) OnFollowing.Invoke();
        }

        m_CurAniST = 
        m_bHurting || m_bAttacking ? m_CurAniST :
        m_groundDetectionData.isGrounded ? Dir != 0 ? 
        VelX == 0 ?                  eMobAniST.Idle : eMobAniST.Walk : 
                                                      eMobAniST.Idle : 
        VelY > 0 ? eMobAniST.AirborneUp : 
                   eMobAniST.AirborneDown;
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

    public void FollowJump()
    {
        m_bFollowJump = true;
        m_bJumpStart = true;
    }

    float FallAndJumpCoolTime = 1f;
    float FallAndJumpCoolTimeT = 1f;
    public bool Follow()
    {
        IsFollowMax = Mathf.Abs(GM.PlayerPos.x - transform.position.x) <= 0.1f;
        IsKeepFollowing = false;
        if (m_bFollowJump) return false;
        FallAndJumpCoolTimeT += Time.deltaTime;
        if ((Mathf.Abs(GM.PlayerPos.y - transform.position.y) >= m_groundDetectionData.Size.y)
            && (transform.position.y > GM.PlayerPos.y)
            && (IsCliff))
        {
            IsKeepFollowing = true;
            if (Fall()) { return true; }
            return true;
        }
        if ((transform.position.y - GM.PlayerPos.y) > m_groundDetectionData.Size.y * 0.9f)
        {
            if (FallAndJumpCoolTimeT >= FallAndJumpCoolTime)
            {
                FallAndJumpCoolTimeT = 0;
                Dir = DirForPlayer;
                if (Fall()) { return true; }
            }
            else
            {
                return false;
            }
        }
        if (m_groundDetectionData.isGrounded)
        {
            if(!IsFollowMax) Dir = DirForPlayer;
            if (GM.PlayerPos.y - transform.position.y >= m_groundDetectionData.Size.y * 0.9f)
            {
                if (IsWallInForword && !IsJumpHeightHitWall) 
                {
                    if (FallAndJumpCoolTimeT >= FallAndJumpCoolTime)
                    {
                        FallAndJumpCoolTimeT = 0;
                        FollowJump();
                    }
                }
            }
            else if (IsOneWayInForword && !IsJumpHeightHitWall) 
            {
                FollowJump();
            }
        }
        
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
    public bool SENoAct()
    {
        return m_SEObj.NoTask;
    }

    public void AttackProcessStart()
    {
        var ColMgr = GetComponentInChildren<AColliderMgr>();
        ColMgr.ReStart(ColMgr.OffSet * -m_MoveData.SprDir);
    }
    public void AttackProcessEnd()
    {
        var ColMgr = GetComponentInChildren<AColliderMgr>();
        ColMgr.Stop();
    }


    public virtual void OnAttackEnd()
    {
    }


    void OnStunnedEnd_AniEvent()
    {
        m_bAttacking = false;
    }

    public override void OnHurt(/* 총알 위치 */)
    {
        if (m_SEObj.Ani != eMobAniST.Last) return; // Ani 우선순위가 Hurt보다 커야함.

        if (m_bAttacking) return;
        if (!m_bHurting)
        {
            m_bPrevDir = m_MoveData.SprDir;
            m_MoveData.SprDir *= (Mathf.Sign(GM.PlayerPos.x - transform.position.x) == m_bPrevDir) ? -1 : 1;
        }
        m_bHurting = true;
        m_bAniStart = true;
        GetComponentInChildren<AColliderMgr>().Stop();
        if (m_SEObj.Ani == eMobAniST.Last) // Ani 우선순위가 Hurt보다 커야함.
            m_CurAniST = eMobAniST.Hit;
    }
    public bool Hurting() => m_bHurting;
    private void HurtEnd()
    {
        m_bAttacking = false;
        atkST = eAtkST.pre;
        m_bHurting = false;
        m_MoveData.SprDir = m_bPrevDir;
        //Debug.Log("End");
    }

    #region Interface: IHurt

    public override void OnDead()
    {
        base.OnDead();
        Destroy(gameObject);
    }
    #endregion

    #region Interface: ICanDetectGround
    virtual public void OnGroundEnter()
    {
        //Debug.Log(m_bFollowJump);
        Jump_Logic.ResetJumpCount(ref m_jumpData);
        m_bFollowJump = false;
        if (m_bHurting) return;
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