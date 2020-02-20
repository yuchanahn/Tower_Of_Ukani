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

    [SerializeField] float CeilingCoolTime = 1f;
    float mCeilingCoolTimeT = 0;

    public void SetCeilingCoolTime()
    {
        mCeilingCoolTimeT = 0;
    }

    private void Awake()
    {
        mMob = GetComponent<Mob_FlowerBat>();
        mCeilingCoolTimeT = CeilingCoolTime;
    }

    public bool Tick()
    {
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
            mMob.Dir2d = Vector2.zero;
            mMob.SetAni(eMobAniST.Hang);
        }
        else
        {
            mMob.Dir2d = (mCeilingPos.Value - (Vector2)transform.position).normalized;
            mMob.SetAni(eMobAniST.Fly);
        }
        
        return true;
    }
}
