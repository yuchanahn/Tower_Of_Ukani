using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBat_Task_Flee : MonoBehaviour, ITask
{
    [SerializeField] public float FleeRange;
    [SerializeField] public float FleeTime;
    [SerializeField] public float AddSpeed;
    [SerializeField] public BoxCollider2D FleeCollider2d;


    private Mob_FlowerBat mMob;

    public bool IsFleeing = false;
    public bool IsFleeAble = false;
    [SerializeField] float fleeTimeT = 0f;

    private void Awake()
    {
        mMob = GetComponent<Mob_FlowerBat>();
    }

    public bool Tick()
    {
        mMob.SetAni(eMobAniST.Flee);
        mMob.mHitImmunity = true;
        IsFleeAble = false;

        mMob.Dir2d = -(GM.PlayerPos - transform.position).normalized;
        mMob.AddSpeed = AddSpeed;
        IsFleeing = true;
        fleeTimeT += Time.deltaTime;
        if (fleeTimeT >= FleeTime)
        {
            fleeTimeT = 0f;
            mMob.AddSpeed = 0;
            mMob.mHitImmunity = false;
            IsFleeing = false;
        }
        return IsFleeing;
    }
}
