using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMob_JPS_Task_Follow : MonoBehaviour, ITask
{
    FlyingMob_Base mMob;
    [SerializeField] JPS_PathFinder mPathFinder;
    JPS_MoveManager mMoveMgr = new JPS_MoveManager();

    private void Awake()
    {
        mMob = GetComponent<FlyingMob_Base>();
    }

    public bool Tick()
    {
        GridView.Inst[1].GetJPS_Path();
        var vel = mMoveMgr.GetVelIfUpdateTarget(mPathFinder, transform.position, GM.PlayerPos, mMob.MoveSpeed);
        mMob.SetJPS_Vel2d(vel);
        mMob.SetAni(eMobAniST.Fly);
        return true;
    }
}