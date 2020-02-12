using System.Collections.Generic;
using UnityEngine;

internal class JPS_MoveManager
{
    Queue<Vector2> mMoveQueue = new Queue<Vector2>();

    public Vector2? GetDir(JPS_PathFinder mPathFinder, Vector2 ori, Vector2? target = null)
    {


        if (mMoveQueue.Count > 0)
        {
            if (Vector2.Distance(ori, mMoveQueue.Peek()) < 0.1f)
            {
                mMoveQueue.Dequeue();
                return (mMoveQueue.Peek() - ori).normalized;
            }
            else
            {
                return (mMoveQueue.Peek() - ori).normalized;
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

    public Vector2 GetDirUpdateTarget(JPS_PathFinder mPathFinder, Vector2 ori, Vector2 target)
    {
        var path = mPathFinder.Find(ori, target)[0];
        if (path == null)
        {
            return Vector2.zero;
        }
        return (path - ori).normalized;
    }
}
