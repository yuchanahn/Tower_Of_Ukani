using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMob_JPS_Task_Follow : MonoBehaviour, ITask
{
    FlyingMob_Base mMob;
    JPS_MoveManager mMoveMgr = new JPS_MoveManager();
    public bool IsFollowing;
    [SerializeField] float offset_y;

    private void Awake()
    {
        mMob = GetComponent<FlyingMob_Base>();
    }

    public bool Tick()
    {
        mMob.MS = FlyingMob_Base.MovementState.JPS_Follow;
        mMob.MovementAction[FlyingMob_Base.MovementState.JPS_Follow] = () =>
        {
            var gv = GridView.Inst[GM.CurMapName][1];
            gv.GetJPS_Path();
            var p1 = gv.GetNodeAtWorldPostiton(GM.PlayerPos).pos;
            var p = p1;
            if (!(GM.CurMapCenter.y + (GM.CurMapSize_Width/2) <= GM.PlayerPos.y + offset_y))
            {
                var p2 = gv.GetNodeAtWorldPostiton(GM.PlayerPos.Add(y: offset_y)).pos;
                p.row--;
                for (; p.row >= p2.row; p.row--)
                {
                    if (gv.GetNodeAtWorldPostiton(p).isObstacle) break;
                }
                p.row++;
                Debug.Log($"{p1.row}, {p.row}");
            }

            var vel = mMoveMgr.GetVelIfUpdateTarget(JPS_PathFinder._1x1, transform.position, gv.getNodePosAsWorldPos(p), mMob.MoveSpeed);
            mMob.SetJPS_Vel2d(vel);
        };
        mMob.SetAni(eMobAniST.Fly);
        return true;
    }
}