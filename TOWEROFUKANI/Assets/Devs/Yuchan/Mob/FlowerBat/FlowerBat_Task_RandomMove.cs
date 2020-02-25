using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALogic;
using Dongjun.Helper;

public class FlowerBat_Task_RandomMove : MonoBehaviour, ITask
{
    [SerializeField] JPS_PathFinder mPathFinder;
    JPS_MoveManager mMoveMgr = new JPS_MoveManager();

    Mob_FlowerBat mMob;

    private bool bIdel = false;
    private bool bMove = true;

    private float idelT = 0;
    private float moveT = 0;

    private void Awake()
    {
        mMob = GetComponent<Mob_FlowerBat>();
    }


    public void ReSet()
    {
        mMoveMgr.Clear();
    }

    public bool Tick()
    {
        mMob.SetAni(eMobAniST.Fly);

        mMob.MS = FlyingMob_Base.MovementState.JPS_RDMove;
        mMob.MovementAction[FlyingMob_Base.MovementState.JPS_RDMove] = () =>
        {
            if (bIdel)
            {
                mMob.Dir2d = Vector2.zero;
                mMob.IsJPSVel = false;
                idelT += Time.deltaTime;
                if (idelT > mMob.MoveTimeIdle)
                {
                    idelT = 0;
                    bIdel = false;
                    bMove = true;
                    mMoveMgr.Clear();
                }
            }
            else if (bMove)
            {
                var dir = mMoveMgr.GetVel(mPathFinder, transform.position, speed: mMob.MoveSpeed);
                if (dir is null)
                {
                    int Range = 3;
                    var pos = GridView.Inst[1].GetNodeAtWorldPostiton(transform.position).pos;

                    List<Point> MoveAblePos = new List<Point>();

                    for (int i = -Range; i < Range; i++)
                    {
                        for (int j = -Range; j < Range; j++)
                        {
                            Point p;
                            p.column = pos.column + i;
                            p.row = pos.row + j;

                            if (p.column <= 0 || p.row <= 0 || (i == 0 && j == 0)) continue;

                            if (!GridView.Inst[1].GetNodeAtWorldPostiton(p).isObstacle)
                                MoveAblePos.Add(p);
                        }
                    }
                    dir = mMoveMgr.GetVel(
                        mPathFinder,
                        transform.position,
                        GridView.Inst[1].getNodePosAsWorldPos(MoveAblePos[Random.Range(0, MoveAblePos.Count)]),
                        mMob.MoveSpeed);
                }
                mMob.SetJPS_Vel2d(dir.GetValueOrDefault());

                moveT += Time.deltaTime;
                if (moveT > mMob.MoveTimeMove)
                {
                    moveT = 0;
                    bMove = false;
                    bIdel = true;
                }
            }
        };
        return true;
    }
    

    void OnDestroy()
    {
        ATimer.Pop(GetInstanceID() + "FindCeiling");
    }
}
