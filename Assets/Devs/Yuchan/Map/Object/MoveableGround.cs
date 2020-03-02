using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableGround : MonoBehaviour
{
    [SerializeField] Vector2Int Size;
    [SerializeField] int TargetMobSize;
    Vector2 mLastPos = Vector2.zero;

    List<Node> mLastNode = new List<Node>();

    private void Awake()
    {
        mLastPos = transform.position;
    }

    void UpdateJPS()
    {
        mLastNode.ForEach(x => x.isObstacle = false);
        var StartPoint = transform.position.Add(-Size.x, -Size.y);

        for (int i = 0; i < Size.x * 2; i++)
        {
            for(int j = 0; j < Size.y * 2; j++)
            {
                GridView.Inst[GM.CurMapName][TargetMobSize].SetGrid(StartPoint.Add(i, j), true);
            }
        }

    }

    private void Update()
    {
        if(Vector2.Distance(mLastPos, transform.position) > 1)
        {
            mLastPos = transform.position;
            UpdateJPS();
        }
    }
}