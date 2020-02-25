using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMob_JPS_Task_Follow : MonoBehaviour, ITask
{
    FlyingMob_Base mMob;
    [SerializeField] public JPS_PathFinder PathFinder;
    JPS_MoveManager mMoveMgr = new JPS_MoveManager();
    public bool IsFollowing;


    private void Awake()
    {
        mMob = GetComponent<FlyingMob_Base>();
    }

    public bool Tick()
    {
        mMob.MovementAction[FlyingMob_Base.MovementState.JPS_Follow] = () =>
        {
            GridView.Inst[1].GetJPS_Path();
            var vel = mMoveMgr.GetVelIfUpdateTarget(JPS_PathFinder._1x1, transform.position, GM.PlayerPos, mMob.MoveSpeed);
            mMob.SetJPS_Vel2d(vel);
        };
        mMob.MS = FlyingMob_Base.MovementState.JPS_Follow;
        mMob.SetAni(eMobAniST.Fly);
        return true;
    }
}