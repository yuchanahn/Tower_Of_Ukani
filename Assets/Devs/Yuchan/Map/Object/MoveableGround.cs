using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableGround : MonoBehaviour
{
    [SerializeField] Vector2Int Size;
    [SerializeField] int TargetMobSize;
    [SerializeField] Vector2 mLastPos = Vector2.zero;
    [SerializeField] Vector2 vel = Vector2.zero;

    List<Node> mLastNode = new List<Node>();

    GridView mCached_GV = null;

    private void Awake()
    {
        
    }
    
    void UpdateJPS(Vector3 vel, float dt)
    {
        mLastNode.ForEach(x => x.isObstacle = false);

        var p = transform.position.Add(-Size.x / 2, -Size.y / 2);

        //TODO : 타겟 사이즈 별로 다 해줘야함....
        // 어쩔수 없다. 걍 박쥐에 콜라이더를 넣자............쉣!!!
        
        p += vel * (1f / dt);
        p += vel * (JPS_PathFinder._1x1.GetComponent<GridView>().last_jps_ms);

        mLastNode = mCached_GV.SetGridRange(
                            p,
                            new Vector2Int(Size.x, Size.y), 
                            true);
    }
    float dt = 0f;
    private void Update()
    {
        if(mCached_GV.IsNull()) mCached_GV = GridView.Inst[GM.CurMapName][TargetMobSize];
        dt += Time.deltaTime;
        if (Mathf.Abs(mLastPos.x - transform.position.x) >= 0.5f || Mathf.Abs(mLastPos.y - transform.position.y) >= 0.5f)
        {
            vel = (Vector2)transform.position - mLastPos;
            mLastPos = transform.position;
            UpdateJPS(vel, dt);
            dt = 0;
        }
    }
}