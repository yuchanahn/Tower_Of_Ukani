using System;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMob_Base : Mob_Base
{
    #region DATA

    [SerializeField] FlyMobMove_ mMoveData;
    [SerializeField] Transform mSprTr;
    #endregion

    public Vector2[] MoveLink;
    int mLinkIdx = 0;
    eAttackST mCurAttackST;
    Rigidbody2D mRb2d;
    Animator mAnimator;
    bool mAniStart;
    protected bool mMoveStop = false;
    protected bool mHitImmunity = false;
    public eMobAniST mCurAniST;
    StatusEffect_Object mSE;

    #region Properties

    int NextLinkIdx => Mathf.Min(mLinkIdx + 1, MoveLink.Length);
    Vector2 NextPath => MoveLink[NextLinkIdx];
    Vector2 Dir2d {
        get
        {
            Vector2 nextPos = transform.position + (Vector3)mMoveData.Dir2d * Time.fixedDeltaTime * MoveSpeed;
            return mHurt        ? Vector2.zero :
                   mAttacking   ? Vector2.zero :
                   mMoveStop    ? Vector2.zero :
                   !( (nextPos.x <  GM.CurMapWorldPoint.x 
                   && nextPos.x > GM.CurMapWorldPoint.x - GM.CurMapSize.width)
                   &&
                   (nextPos.y <  GM.CurMapWorldPoint.y 
                   && nextPos.y > GM.CurMapWorldPoint.y - GM.CurMapSize.height)) ? Vector2.zero :

                   mMoveData.Dir2d;
        }

        set { if (mSE.SEChangeDirAble) mMoveData.Dir2d = value; }
    }
    public Vector3 Pos => transform.position - (Vector3.up * 3);
    public Vector2 Pos2d => transform.position - (Vector3.up * 3);
    float MoveSpeed => mMoveData.Speed;
    public bool IsHurting => mHurt;
    public int SpriteDir
    {
        get
        {
            return mSprTr.eulerAngles.y == 180 ? -1 : 1;
        }
        set
        {
            mSprTr.eulerAngles = new Vector2(0, value == -1 ? 0 : 180);
        }
    }



    #endregion


    virtual protected void Awake()
    {
        //======================================================================
        //      ## StatusEffect
        //======================================================================

        mSE = GetComponent<StatusEffect_Object>();

        //======================================================================

        Init_AniDuration();   // 애니 시간.

        mRb2d = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();

        mAttack[eAttackST.PreAttack] = PreAttack;
        mAttack[eAttackST.Attack] = Attack;
    }

    void Animation()
    {
        if (mSE.SEAni != eMobAniST.Last)
        {
            // TODO : 
            // 상태이상이 애니를 쓴다면. 다른 애니 끝내자.
            // 
            // 
            mCurAniST = mSE.SEAni;
            HurtEnd();
            return;
        }

        if (!mHurt) SpriteDir = Dir2d.x > 0 ? 1 : -1;
    }

    void FixedUpdate()
    {
        
        mRb2d.velocity = Dir2d * MoveSpeed * mSE.SESpeedMult * (CheckOverlapSlow(MobSize, Dir2d) ? OverlapSlow : 1f);
        Animation();
        Anim_Logic.SetAnimSpeed(mAnimator, m_Ani[mCurAniST].Item2);
        if (mAniStart) { mAnimator.Play(m_Ani[mCurAniST].Item1, 0, 0); mAniStart = false; }
        else mAnimator.Play(m_Ani[mCurAniST].Item1);
        
    }

    public virtual bool Stunned()
    {
        return true;
    }


    //======================================================================
    //      ## Attack
    //======================================================================

    [Header("Attack"), Range(0,100f)] public float AttackRange;

    protected bool mAttacking = false;
    public bool IsAttacking => mAttacking;

    virtual public bool OnAttack()
    {
        mAttack[mCurAttackST]();

        return true;
    }

    virtual protected void OnAttackEnd()
    {
        mAttacking = false;
        mCurAttackST = eAttackST.PreAttack;
    }

    virtual protected void PreAttack()
    {
        mAttacking = true;
        mCurAniST = eMobAniST.Attack_Pre;
    }

    void PreAttackEnd_AniEvent()
    {
        mCurAttackST = eAttackST.Attack; 
        OnAttackStart();
    }

    virtual protected void OnAttackStart()
    {

    }

    virtual protected void Attack()
    {
        mCurAniST = eMobAniST.Attack_Post;
    }

    void AttackEnd_AniEvent()
    {
        OnAttackEnd();
    }







    //=================================================================
    //      ## Movement
    //=================================================================

    // # Rnadom Move
    // 이동주기
    [Header("MovementRate")]
    [SerializeField] float MoveTimeIdle = 1f;
    [SerializeField] float MoveTimeMove = 3f;

    float MoveTime = 3f;
    float MoveT = 0;

    // true = Move;
    // false = Idle;
    bool MoveST = true;
    // 상태 시작시 한번만 실행될꺼 검사.
    bool StateFrist = false;

    public bool RandomMove()
    {
        if(MoveST)  // ● MOVE  
        {
            if (StateFrist)
            {
                StateFrist = false;
                Dir2d = ARandom.Dir2d;
            }
            mCurAniST = eMobAniST.Fly;
        }
        else        // ● IDLE
        {
            Dir2d = Vector2.zero;
            mCurAniST = eMobAniST.Idle;
        }

        MoveT += Time.deltaTime;
        if (MoveT > MoveTime)
        {
            MoveT = 0;
            MoveST = !MoveST;
            MoveTime = MoveST ? MoveTimeMove : MoveTimeIdle;
            StateFrist = true;
        }
        return true;
    }

    // # Follow
    // 이동 
    [Header("AgroRange")]
    [SerializeField] public float AgroRange;
    public bool Follow()
    {
        Dir2d = (GM.PlayerPos - Pos).normalized;
        mCurAniST = eMobAniST.Fly;
        return true;
    }


    //=================================================================
    //      ## Hit
    //=================================================================

    bool mHurt = false;

    protected void HurtEnd()
    {
        mHurt = false;
    }

    public override void OnHurt()
    {
        if (mHitImmunity) return;
        mHurt       = true;
        mAniStart   = true;
        mCurAniST   = eMobAniST.Hit;

        SpriteDir = (GM.PlayerPos.x - transform.position.x) < 0 ? -1 : 1;
    }
    public void HurtEnd_AniEvent()
    {
        HurtEnd();
    }


    //=================================================================
    //      ## Dead
    //=================================================================

    [SerializeField, Header("Compse")] CorpseData Compse;
    public override void OnDead()
    {
        Destroy(gameObject);
        CorpseMgr.CreateCorpseOrNull(transform, Compse);
    }
}
