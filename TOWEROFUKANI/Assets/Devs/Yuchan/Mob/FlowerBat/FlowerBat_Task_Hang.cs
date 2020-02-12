using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBat_Task_Hang : MonoBehaviour, ITask
{
    Mob_FlowerBat mMob;

    public float mCeilingDetectRange;
    public Vector2? mCeilingPos = null;
    public bool IsFollowEndToCeiling = false;
    public bool FindCeiling = false;
    public bool IsCeilingNear() => Vector2.Distance(transform.position, mCeilingPos.GetValueOrDefault()) < 0.1f;
    public static List<GameObject> HangWalls = new List<GameObject>();

    private void Awake()
    {
        mMob = GetComponent<Mob_FlowerBat>();
    }

    public bool Tick()
    {
        if (IsFollowEndToCeiling)
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
}
