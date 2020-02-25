using System.Collections.Generic;
using UnityEngine;

internal class JPS_MoveManager
{
    Queue<Vector2> mMoveQueue = new Queue<Vector2>();
    public void Clear()
    {
        mMoveQueue.Clear();
    }
    public Vector2? GetVel(JPS_PathFinder mPathFinder, Vector2 ori, Vector2? target = null, float speed = 0f)
    {
        GridView.Inst[1].GetJPS_Path();
        
        if (mMoveQueue.Count > 0)
        {
            var tpos = mMoveQueue.Peek();
            if (tpos == ori)
            {
                mMoveQueue.Dequeue();
                return (tpos - ori).normalized * Mathf.Clamp(speed, 0, Vector2.Distance(tpos, ori) / Time.fixedDeltaTime);
            }
            else
            {
                return (tpos - ori).normalized * Mathf.Clamp(speed, 0, Vector2.Distance(tpos, ori) / Time.fixedDeltaTime);
            }
        }
        else
        {
            if (target is null)
            {
                return null;
            }
            var path = mPathFinder.Find(ori, target.GetValueOrDefault());
            foreach (var i in path)
            {
                mMoveQueue.Enqueue(i);
            }
            if(mMoveQueue.Count == 0)
            {
                return Vector2.zero;
            }
            return (mMoveQueue.Peek() - ori).normalized * Mathf.Clamp(speed, 0, Vector2.Distance(mMoveQueue.Peek(), ori) / Time.fixedDeltaTime);
        }
    }

    Vector2? CurGoal = null;

    public Vector2 GetVelIfUpdateTarget(JPS_PathFinder mPathFinder, Vector2 origin, Vector2 target, float speed)
    {
        var tposList = mPathFinder.Find(origin, target);
        if (tposList.Length == 0)
        {
            return Vector2.zero;
        }

        var Goal = tposList[0];

        bool SetGoal(int n)
        {
            if (tposList.Length > n)
            {
                Goal = tposList[n];
            }
            else
            {
                return false;
            }
            return true;
        }

        if (CurGoal != null && CurGoal.Value == Goal)
        {
            if (!SetGoal(1))
            {
                return Vector2.zero;
            }
        }
        if (origin == Goal)
        {
            CurGoal = origin;
            if (!SetGoal(1))
            {
                return Vector2.zero;
            }
        }

        return (Goal - origin).normalized * Mathf.Clamp(speed, 0, Vector2.Distance(origin, Goal) / Time.fixedDeltaTime);
    }
}
