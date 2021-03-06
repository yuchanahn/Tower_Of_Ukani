﻿using Dongjun.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlyingMob_Base : Mob_Base
{
    #region DATA

    [SerializeField] FlyMobMove_Data mMoveData;
    [SerializeField] public float AddSpeed = 0f;
    [SerializeField] Transform mSprTr;
    #endregion

    public Vector2[] MoveLink;
    int mLinkIdx = 0;
    eAttackST mCurAttackST;
    Rigidbody2D mRb2d;
    Animator mAnimator;
    bool mAniStart;
    protected bool mMoveStop = false;
    public bool mHitImmunity = false;
    public eMobAniST mCurAniST;
    StatusEffect_Object mSE;

    #region Properties

    int NextLinkIdx => Mathf.Min(mLinkIdx + 1, MoveLink.Length);
    Vector2 NextPath => MoveLink[NextLinkIdx];
    public FlyMobMove_Data MoveData => mMoveData;
    public Vector2 Dir2d {
        get
        {
            Vector2 nextPos = transform.position + (Vector3)mMoveData.Dir2d * Time.fixedDeltaTime * MoveSpeed;
            return mHurt        ? Vector2.zero :
                   mAttacking   ? Vector2.zero :
                   mMoveStop    ? Vector2.zero :
                   IsJPSVel     ? JPS_Vel      :
                   !( (nextPos.x <  GM.CurMapWorldPoint.x 
                   && nextPos.x > GM.CurMapWorldPoint.x - GM.CurMapSize_Width)
                   &&
                   (nextPos.y <  GM.CurMapWorldPoint.y 
                   && nextPos.y > GM.CurMapWorldPoint.y - GM.CurMapSize_Height)) ? Vector2.zero :

                   mMoveData.Dir2d;
        }

        set { if (mSE.ChangeDirAble) mMoveData.Dir2d = value; }
    }
    public Vector3 Pos => transform.position - (Vector3.up * 3);
    public Vector2 Pos2d => transform.position - (Vector3.up * 3);
    public virtual float MoveSpeed => mMoveData.Speed + AddSpeed;
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
    public enum MovementState
    {
        JPS_Follow,
        JPS_RDMove,
        NoJPS,
    }

    public MovementState MS = MovementState.NoJPS;

    public Dictionary<MovementState, Action> MovementAction = new Dictionary<MovementState, Action>();

    public bool IsJPSVel = false;
    Vector2 JPS_Vel = Vector2.zero;

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

        MovementAction[MovementState.NoJPS] = () => IsJPSVel = false;
        MovementAction[MovementState.JPS_Follow] = () => { };
        MovementAction[MovementState.JPS_RDMove] = () => { };
    }
    void Animation()
    {
        if (mSE.Ani != eMobAniST.Last)
        {
            mCurAniST = mSE.Ani;
            SetStatusEffectUseAniState();
            return;
        }

        if (!mHurt) SpriteDir = Dir2d.x > 0 ? 1 : -1;
    }


    protected virtual void SetStatusEffectUseAniState()
    {
        mHurt = false;
        BTStop = false;
        AddSpeed = 0;
        // Not Test....
        mHitImmunity = false;
    }



    
    
    public void MovementStop()
    {
        Dir2d = Vector2.zero;
        MS = MovementState.NoJPS;
    }
    public void SetJPS_Vel2d(Vector2 vel)
    {
        IsJPSVel = true;
        JPS_Vel = vel;
        SpriteDir = vel.x > 0 ? 1 : -1;
    }



    eMobAniST prevAni = eMobAniST.Last;



    float ani_noamltime_of_last = 0f;
    private void OnEnable()
    {
        mAnimator.Play(m_Ani[mCurAniST].Item1, 0, ani_noamltime_of_last);
    }

    private void OnDisable()
    {
        ani_noamltime_of_last = mAnimator.GetNormalizedTime();
    }

    float CurveTime = 0f;
    virtual protected void FixedUpdate()
    {
        MovementAction[MS]();

        mRb2d.velocity = Dir2d * (IsJPSVel ? 1f : MoveSpeed) * mSE.SpeedMult * (CheckOverlapSlow(MobSize, Dir2d) ? OverlapSlow : 1f);
        if(mSE.UseSEVel)
        {
            mRb2d.velocity = mSE.EffectDir2d * mSE.SpeedMult;
        }
        if (!mSE.UseSEVelCurve) CurveTime = 0f;
        if (mSE.UseSEVelCurve)
        {
            mRb2d.velocity = mSE.EffectDir2d * mSE.VelCurve.Evaluate(CurveTime);
            CurveTime += Time.fixedDeltaTime;
        }

        Animation();
        mAnimator.SetDuration(m_Ani[mCurAniST].Item2);
        if (mAniStart)
        {
            mAnimator.Play(m_Ani[mCurAniST].Item1, 0, 0);
            mAniStart = false;
        }
        else
        {
            if (prevAni != mCurAniST)
            {
                prevAni = mCurAniST;
                mAnimator.Play(m_Ani[mCurAniST].Item1);
            }
        }
        IsJPSVel = false;
    }





    public virtual bool Stunned()
    {
        return true;
    }


    public virtual void SetAni(eMobAniST ani)
    {
        if (mAnimator.speed == 0)
        {
            AniPlay();
        }
        mCurAniST = ani;
    }
    public void AniStop()
    {
        mAnimator.speed = 0;
    }
    public void AniPlay()
    {
        mAnimator.speed = 1;
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
    [SerializeField] public float MoveTimeIdle = 1f;
    [SerializeField] public float MoveTimeMove = 3f;

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

    protected virtual void HurtEnd()
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

    public override void OnDead()
    {
        base.OnDead();
        Destroy(gameObject);
    }
}
