using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum eAttackST
{
    PreAttack,
    Attack
}

public class FlyingMob_Base : MonoBehaviour, IHurt
{
    #region DATA

    [SerializeField] FlyMobMove_ mMoveData;
    [SerializeField] AniSpeedData[] mAniSpeedData;
    [SerializeField] Transform mSprTr;


    #endregion


    public Vector2[] MoveLink;
    int mLinkIdx = 0;
    Dictionary<eAttackST, Action> mAttack = new Dictionary<eAttackST, Action>();
    eAttackST mCurAttackST;
    Rigidbody2D mRb2d;
    Animator mAnimator;
    Dictionary<eMobAniST, (string, float)> m_Ani = new Dictionary<eMobAniST, (string, float)>();
    bool mAniStart;
    eMobAniST mCurAniST;
    StatusEffect_Object mSE;

    #region Properties

    int NextLinkIdx => Mathf.Min(mLinkIdx + 1, MoveLink.Length);
    Vector2 NextPath => MoveLink[NextLinkIdx];
    Vector2 Dir2d { 
        get 
        { 
            return mHurt ? Vector2.zero :
                           mMoveData.Dir2d;
        } 

        set { if (mSE.SEChangeDirAble) mMoveData.Dir2d = value; } 
    }
    Vector2 Pos2d => transform.position;
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


    private void Awake()
    {
        //======================================================================
        //      ## Init
        //======================================================================

        mSE = GetComponent<StatusEffect_Object>();

        //======================================================================

        for (int i = 0; i < (int)eMobAniST.Last; i++)
            m_Ani[(eMobAniST)i] = ($"{gameObject.name}_{((eMobAniST)i).ToString()}", 0);
        foreach (var i in mAniSpeedData)
            m_Ani[i.ST] = ($"{gameObject.name}_{i.ST.ToString()}", i.t);

        mRb2d     = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();

        mAttack[eAttackST.PreAttack]    = PreAttack;
        mAttack[eAttackST.Attack]       = Attack;
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

        if(!mHurt) SpriteDir = Dir2d.x > 0 ? 1 : -1;
    }

    void FixedUpdate()
    {
        mRb2d.velocity = Dir2d * MoveSpeed * mSE.SESpeedMult;
        Animation();
        Anim_Logic.SetAnimSpeed(mAnimator, m_Ani[mCurAniST].Item2);
        if (mAniStart) { mAnimator.Play(m_Ani[mCurAniST].Item1, 0, 0); mAniStart = false; }
        else mAnimator.Play(m_Ani[mCurAniST].Item1);
    }



    void PreAttack()
    {

    }
    void PreAttackEnd_AniEvent()
    {
        mCurAttackST = eAttackST.Attack;
    }

    void Attack()
    {

    }
    void AttackEnd_AniEvent()
    {
        mCurAttackST = eAttackST.PreAttack;
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
        Dir2d = (GM.PlayerPos - transform.position).normalized;
        return true;
    }


    bool mHurt = false;

    void HurtEnd()
    {
        mHurt = false;
    }

    public void OnHurt()
    {
        mHurt       = true;
        mAniStart   = true;
        mCurAniST   = eMobAniST.Hit;

        SpriteDir = (GM.PlayerPos.x - transform.position.x) < 0 ? -1 : 1;
    }
    public void HurtEnd_AniEvent()
    {
        HurtEnd();
    }

    public void OnDead()
    {
        Debug.Log("Dead!");
    }
}
