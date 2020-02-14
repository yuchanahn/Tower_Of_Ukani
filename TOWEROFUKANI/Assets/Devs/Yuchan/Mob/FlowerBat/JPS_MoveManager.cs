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
    public Vector2 GetDirIfUpdateTarget(JPS_PathFinder mPathFinder, Vector2 ori, Vector2 target)
    {
        var tposList = mPathFinder.Find(ori, target);

        if (tposList.Length > 0)
        {
            float val = 1;
            return (tposList[0] - ori).normalized * Mathf.Clamp(val, 0, Vector2.Distance(tposList[0], ori) / Time.fixedDeltaTime);
        }
        return Vector2.zero;
    }
}
