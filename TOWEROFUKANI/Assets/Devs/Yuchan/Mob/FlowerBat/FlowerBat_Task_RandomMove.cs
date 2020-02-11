using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALogic;
using Dongjun.Helper;

public class FlowerBat_Task_RandomMove : MonoBehaviour, ITask
{
    [SerializeField] float mCeilingDetectRange;
    [SerializeField] JPS_PathFinder mPathFinder;

    Queue<Vector2> mMoveQueue = new Queue<Vector2>();

    Mob_FlowerBat mMob;
    Vector2? mCeilingPos = null;
    bool bFindCeilingPos = false;
    bool Check = false;

    private bool bIdel = false;
    private bool bMove = true;

    private float idel_dt = 0;
    private float move_dt = 0;


    static List<GameObject> HangWalls = new List<GameObject>();

    public bool IsDetectedCeiling()
    {
        var Tr = Detect.GetHitTrOrNull(transform.position, Vector2.up, mCeilingDetectRange, GM.SoildGroundLayer);
        if (Tr) 
        {
            if(HangWalls.Contains(Tr.gameObject))
            {
                return false;
            }
            mCeilingPos = Tr.position.Foot(Vector2.one).Add(y: -mMob.Size.y/2);
        }
        else
        {
            return false;
        }

        HangWalls.Add(Tr.gameObject);

        return true;
    }
    bool InCeiling => Vector2.Distance(transform.position, mCeilingPos.GetValueOrDefault()) < 0.1f;

    

    private void Awake()
    {
        mMob = GetComponent<Mob_FlowerBat>();
    }

    public bool Tick()
    {
        if (bFindCeilingPos)
        {
            if (InCeiling)
            {
                mMob.Dir2d = Vector2.zero;
                mMob.SetAni(eMobAniST.Hang);
            }
            else
            {
                mMob.Dir2d = (mCeilingPos.Value - (Vector2)transform.position).normalized;
                
            }
            return true;
        }
        if (IsDetectedCeiling())
        {
            bFindCeilingPos = true;
        }
        //if (Check)
        //{
        //    if (ARandom.Get(50))
        //    {
        //        Check = false;
        //        ATimer.Set(GetInstanceID() + "FindCeiling", 1f, () => Check = true);
        //    }
        //
        //}

        if(bIdel)
        {
            mMob.Dir2d = Vector2.zero;
            idel_dt += Time.deltaTime;
            if(idel_dt > mMob.MoveTimeIdle)
            {
                idel_dt = 0;
                bIdel = false;
                bMove = true;
            }
        }
        else if(bMove)
        {
            if(mMoveQueue.Count > 0)
            {
                if(Vector2.Distance(transform.position, mMoveQueue.Peek()) < 0.1f)
                {
                    mMoveQueue.Dequeue();
                }
                else
                {
                    mMob.Dir2d = (mMoveQueue.Peek() - (Vector2)transform.position).normalized;
                }
            }
            else
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
                GridView.Inst[1].GetJPS_Path();
                var path = mPathFinder.Find(transform.position, GridView.Inst[1].getNodePosAsWorldPos(MoveAblePos[Random.Range(0, MoveAblePos.Count)]));
                foreach (var i in path)
                {
                    mMoveQueue.Enqueue(i);
                }
                mMob.Dir2d = (mMoveQueue.Peek() - (Vector2)transform.position).normalized;
            }


            move_dt += Time.deltaTime;
            if(move_dt > mMob.MoveTimeMove)
            {
                move_dt = 0;
                bMove = false;
                bIdel = true;
            }
        }


        return true;
    }
    

    void OnDestroy()
    {
        ATimer.Pop(GetInstanceID() + "FindCeiling");
    }
}
