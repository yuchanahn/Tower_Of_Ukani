using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableGround : MonoBehaviour
{
    [SerializeField] Vector2Int Size;
    [SerializeField] int TargetMobSize;
    [SerializeField] Vector2 mLastPos = Vector2.zero;

    List<Node> mLastNode = new List<Node>();

    GridView mCached_GV = null;

    private void Awake()
    {
        
    }

    void UpdateJPS()
    {
        mLastNode.ForEach(x => x.isObstacle = false);
        mLastNode = mCached_GV.SetGridRange(
                            transform.position.Add(-Size.x/2, -Size.y/2),
                            new Vector2Int(Size.x, Size.y), 
                            true);
    }

    private void Update()
    {
        if(mCached_GV.IsNull()) mCached_GV = GridView.Inst[GM.CurMapName][TargetMobSize];

        if (Mathf.Abs(mLastPos.x - transform.position.x) >= 0.5f || Mathf.Abs(mLastPos.y - transform.position.y) >= 0.5f)
        {
            mLastPos = transform.position;
            UpdateJPS();
        }
    }
}