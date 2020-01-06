using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_WeepingMist : FlyingMob_Base
{
    [SerializeField, Header("Teleport"), Range(0, 100)] 
    int RandRangeOfTeleport;

    //=================================================================
    //      ## Attack : override
    //=================================================================

    [SerializeField, Header("AttackDuration")] float AttackDuration;
    [SerializeField] GameObject RainObject;
    [SerializeField] SpriteRenderer mRainRenderer;
    [SerializeField] BoxCollider2D RainCollider;
    [SerializeField] Vector2 RainSize;

    override protected void PreAttack()
    {
        mHitImmunity = true;
        mAttacking = true;
        mCurAniST = eMobAniST.PreCry;
    }
    protected override void Attack()
    {
        mCurAniST = eMobAniST.Cry;

        RainObject.SetActive(true);
        RainObject.transform.localPosition = new Vector3(0, -RainSize.y/2);
        RainCollider.size = RainSize;
        mRainRenderer.size = RainSize;
    }
    protected override void OnAttackEnd()
    {
        
    }

    void AttackStop()
    {
        mHitImmunity = false;
        base.OnAttackEnd();
        RainObject.SetActive(false);
    }

    protected override void OnAttackStart()
    {
        base.OnAttackStart();
        ATimer.Set(GetInstanceID() + "AttackEndTimer", AttackDuration, AttackStop);
    }
    private void OnDestroy()
    {
        ATimer.Pop(GetInstanceID() + "AttackEndTimer");
    }
    //=================================================================
    //      ## Teleport
    //=================================================================

    public bool IsTeleporting => mTeleport;
    bool mTeleport = false;

    void OnTeleportPre()
    {
        StartCoroutine(OneFream(() =>
        {
            if (GetComponent<StatusEffect_Stunned>()) return;
            HurtEnd();
            mHitImmunity = true;
            mMoveStop = true;
            mTeleport = true;
            mCurAniST = eMobAniST.Spread;
        }));
    }
    void OnTeleportPreEnd()
    {
        OnTeleport(GM.PlayerPos + new Vector3(0, 3));
    }
    void TeleportPreEnd_AniEvent() => OnTeleportPreEnd();

    void OnTeleport(Vector2 Target)
    {
        StatusEffect_IgnoreHit.Create(gameObject);
        transform.position = Target + Random.insideUnitCircle * 2f;
        mCurAniST = eMobAniST.Merge;
    }
    void OnTeleportEnd()
    {
        GetComponent<StatusEffect_IgnoreHit>().Destroy();
        mHitImmunity = true;
        mMoveStop = false;
        mTeleport = false;
        OnAttack();
    }
    void TeleportEnd_AniEvent() => OnTeleportEnd();

    //=================================================================
    //      ## Stunned : override
    //=================================================================

    public override bool Stunned()
    {
        base.Stunned();
        AttackStop();
        return true;
    }

    //=================================================================
    //      ## Hurt : override
    //=================================================================

    public override void OnHurt()
    {
        if (mHitImmunity) return;
        if(ARandom.Get(RandRangeOfTeleport))    OnTeleportPre();
        else                                    base.OnHurt();
        //base.OnHurt();
        //OnTeleportPre();
    }

    IEnumerator OneFream(System.Action act)
    {
        yield return new WaitForEndOfFrame();
        act();
    }
}