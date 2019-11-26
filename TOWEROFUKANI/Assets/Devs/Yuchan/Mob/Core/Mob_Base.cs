using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum eMobAniST
{
    Attack_Pre,
    Attack_Post,
    Hit,
    Walk,
    Idle,
    AirborneDown,
    AirborneUp,
    Stunned,
    Last,
}

[Serializable]
public struct AniSpeedData
{
    public eMobAniST ST;
    public float t;
}

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

public class Mob_Base : MonoBehaviour, IHurt, ICanDetectGround
{
    #region Var: Inspector

    [Header("Collision")]
    [SerializeField] BoxCollider2D m_OneWayCollider;

    [SerializeField] protected eMobAniST m_CurAniST;

    [Header("Attack")]
    [SerializeField] float m_AttackRange;
    [SerializeField] Vector2 m_AttackSize;
    [SerializeField] Vector2 m_AttackOffset;

    [Header("Data")]
    [SerializeField] AniSpeedData[] m_AniSpeedData;
    [SerializeField] MobMoveData m_MoveData;
    [SerializeField] FollowData m_followData;
    [SerializeField] CorpseData m_compseData;
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
    protected int Dir { set { if(m_SEObj.SEChangeDirAble) m_MoveData.Dir = value; } get { return m_MoveData.Dir; } }

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

    public bool IsOneWayInForword => !CliffDetect_Logic.CanGo(
    transform.position,
    Dir,
    m_MoveData.Speed,
    m_groundDetectionData.Size,
    m_followData.CantMoveGround);

    public virtual bool CanFollow =>
         !m_SEObj.SEFallowAble ? false :
         Hurting() ? false :
         ((GM.PlayerPos - transform.position).magnitude < m_followData.dist);

    public virtual bool CanAttack =>
        !m_SEObj.SEAttackAble ? false : 
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

        m_rb.velocity = new Vector2(m_bFollowJump && m_jumpData.isJumping ? JumpVel.x : VelX * m_SEObj.SESpeedMult, VelY);
        m_groundDetectionData.FallThrough(ref m_bFallStart, m_rb, transform, m_OneWayCollider);

        
        m_jumpData.Jump(ref m_bJumpStart, m_rb, transform);


        Gravity_Logic.ApplyGravity(m_rb, m_groundDetectionData.isGrounded ? new GravityData(false, 0, 0) :
            m_bFollowJump ? m_gravityData :
            !m_jumpData.isJumping ? m_gravityData : new GravityData(true, m_jumpData.jumpGravity, 0));


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
        // 상태이상이 애니를 쓴다면...?
        // 상태 바꾸고 바로 끝내.
        EndFollowing.Invoke();
        if (m_SEObj.SEAni != eMobAniST.Last)
        {
            // TODO : 
            // 상태이상이 애니를 쓴다면. 다른 애니 끝내자.
            // 
            // 
            m_CurAniST = m_SEObj.SEAni;
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


    public bool Follow()
    {
        IsFollowMax = false;
        IsKeepFollowing = false;
        if (m_bFollowJump) return false;


        //var pathFind = PathFinder.Inst.FindPath(transform.position, transform.position.GetGorundOfBottomPos(m_groundDetectionData.Size, m_followData.CantMoveGround), GM.PlayerPos.GetGorundOfBottomPos(GM.PlayerSize, m_followData.CantMoveGround));
        //if (!pathFind.bFollow) return false;
        //Dir = pathFind.bJump ? FollowJump(pathFind.nomal) : pathFind.nomal.x < 0 ? -1 : 1;

        //if (Mathf.Abs(GM.PlayerPos.y - transform.position.y) >= m_groundDetectionData.Size.y) return false;
        //if (transform.position.RayHit(GM.PlayerPos, m_followData.CantMoveGround)) return false;

        if ((Mathf.Abs(GM.PlayerPos.y - transform.position.y) >= m_groundDetectionData.Size.y)
            && (transform.position.y > GM.PlayerPos.y)
            && (IsCliff))
        {
            IsKeepFollowing = true;
            return true;
        }
        
        if ((transform.position.y - GM.PlayerPos.y) > m_groundDetectionData.Size.y * 0.9f)
        {
            Dir = DirForPlayer;
            if (Fall())
            {
                return true;
            }
        }
        if (m_groundDetectionData.isGrounded)
        {
            Dir = DirForPlayer;
            if (GM.PlayerPos.y - transform.position.y >= m_groundDetectionData.Size.y * 0.9f)
            {
                // 일단 우드 플렛폼의 Y사이즈가 이상함;
                // 그거떄매 땅에서 벽으로 감지하고 올라올라함;
                // ....?
                
                if (IsWallInForword) { FollowJump(); }
            }
            else if (IsOneWayInForword) { FollowJump(); }
        }
        IsFollowMax = Mathf.Abs(GM.PlayerPos.x - transform.position.x) <= 0.1f;

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
        return m_SEObj.SENoTask;
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

    public virtual void OnHurt(/* 총알 위치 */)
    {
        if (m_SEObj.SEAni != eMobAniST.Last) return; // Ani 우선순위가 Hurt보다 커야함.

        if (m_bAttacking) return;
        if (!m_bHurting)
        {
            m_bPrevDir = m_MoveData.SprDir;
            m_MoveData.SprDir *= (Mathf.Sign(GM.PlayerPos.x - transform.position.x) == m_bPrevDir) ? -1 : 1;
        }
        m_bHurting = true;
        m_bAniStart = true;
        GetComponentInChildren<AColliderMgr>().Stop();
        if (m_SEObj.SEAni == eMobAniST.Last) // Ani 우선순위가 Hurt보다 커야함.
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

    public virtual void OnDead()
    {
        Destroy(gameObject);
        CorpseMgr.CreateCorpseOrNull(transform, m_compseData);
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