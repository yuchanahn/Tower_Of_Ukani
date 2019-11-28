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

    #region Properties

    int NextLinkIdx => Mathf.Min(mLinkIdx + 1, MoveLink.Length);
    Vector2 NextPath => MoveLink[NextLinkIdx];
    Vector2 Dir2d { get { return mMoveData.Dir2d; } set { mMoveData.Dir2d = value; } }
    Vector2 Pos2d => transform.position;
    float MoveSpeed => mMoveData.Speed;
    public bool IsHurting => mHurt;

    #endregion


    private void Awake()
    {
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

    }

    void FixedUpdate()
    {
        mRb2d.velocity = Dir2d * MoveSpeed;
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


    // 이동주기
    float MoveTimeIdle = 1f;
    float MoveTimeMove = 3f;

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
