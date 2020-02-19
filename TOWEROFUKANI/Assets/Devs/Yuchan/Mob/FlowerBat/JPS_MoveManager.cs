using System.Collections.Generic;
using UnityEngine;

internal class JPS_MoveManager
{
    Queue<Vector2> mMoveQueue = new Queue<Vector2>();
    public Vector2? GetDir(JPS_PathFinder mPathFinder, Vector2 ori, Vector2? target = null)
    {
        if (mMoveQueue.Count > 0)
        {
            var tpos = mMoveQueue.Peek();
            if (Vector2.Distance(ori, tpos) < 0.1f)
            {
                mMoveQueue.Dequeue();
                return (tpos - ori).normalized;
            }
            else
            {
                return (tpos - ori).normalized;
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
            return (mMoveQueue.Peek() - ori).normalized;
        }
    }

    Vector2? CurGoal = null;

    public Vector2 GetDirIfUpdateTarget(JPS_PathFinder mPathFinder, Vector2 origin, Vector2 target)
    {
        var tposList = mPathFinder.Find(origin, target);

        if(tposList.Length == 0)
        {
            return Vector2.zero;
        }

        var Goal = tposList[0];

        if (CurGoal != null && CurGoal.Value == tposList[0])
        {
            if (tposList.Length > 1)
            {
                Goal = tposList[1];
            }
            else
            {
                return Vector2.zero;
            }
                
        }
        //if (origin >= Goal)
        {
            CurGoal = origin;
        }

        return (Goal - origin).normalized;
    }
}
