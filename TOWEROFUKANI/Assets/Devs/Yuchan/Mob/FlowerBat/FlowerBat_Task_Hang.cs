using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlowerBat_Task_Hang : MonoBehaviour, ITask
{
    Mob_FlowerBat mMob;

    public Transform MyCeiling = null;
    public float mCeilingDetectRange;

    public Vector2? mCeilingPos = null;
    public bool IsFollowEndToCeiling = false;
    public bool FindCeiling = false;
    public bool IsCeilingNear() => Vector2.Distance(transform.position, mCeilingPos.GetValueOrDefault()) < 0.1f;
    public bool IsCeilingNearOf() => mCeilingPos is null ? true : Vector2.Distance(transform.position, mCeilingPos.GetValueOrDefault()) > mCeilingDetectRange;
    public static List<GameObject> HangWalls = new List<GameObject>();

    [SerializeField, Range(0, 100)] public int CeilingPercentage;
    [SerializeField] public float CeilingCoolTime = 1f;
    [SerializeField] Vector2 mHangSize;
    [SerializeField] float RangeOfWakeAround;
    float mCeilingCoolTimeT = 0;

    public void WakeAround()
    {
        mCeilingCoolTimeT = 0;


        foreach (var i in 
            Physics2D.CircleCastAll(transform.position, RangeOfWakeAround, Vector2.zero)
            .Map(x => x.collider.gameObject)
            .Filter(x => x.GetComponent<FlowerBat_Task_Hang>()))
        {
            i.GetComponent<FlowerBat_Task_Hang>().mCeilingCoolTimeT = 0f;
        }
    }



    private void Awake()
    {
        mMob = GetComponent<Mob_FlowerBat>();
        mCeilingCoolTimeT = CeilingCoolTime;
    }
    void HangStart_AniEvent()
    {
        mMob.GetComponent<BoxCollider2D>().size = mHangSize;
    }
    void HangEnd_AniEvent()
    {
        
    }
    public bool Tick()
    {
        mMob.MS = FlyingMob_Base.MovementState.NoJPS;
        mCeilingCoolTimeT += Time.deltaTime;
        if (mCeilingCoolTimeT < CeilingCoolTime)
        {
            if (MyCeiling != null)
                HangWalls.Remove(MyCeiling.gameObject);
            MyCeiling = null;
            mCeilingPos = null;
            IsFollowEndToCeiling = false;
            mMob.mHitImmunity = false;
            mMob.SetAni(eMobAniST.Unhang);
            return false;
        }

        if (IsFollowEndToCeiling)
        {
            mMob.mHitImmunity = true;
            mMob.MovementStop();
            mMob.SetAni(eMobAniST.Hang);
        }
        else
        {
            mMob.MS = FlyingMob_Base.MovementState.NoJPS;
            mMob.Dir2d = (mCeilingPos.Value - (Vector2)transform.position).normalized;
            mMob.SetAni(eMobAniST.Fly);
        }
        
        return true;
    }
}
