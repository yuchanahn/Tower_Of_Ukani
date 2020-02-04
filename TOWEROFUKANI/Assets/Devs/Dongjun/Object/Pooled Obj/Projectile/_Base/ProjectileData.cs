using System;
using UnityEngine;

public struct ProjectileData
{
    public FloatStat moveSpeed;
    public FloatStat travelDist;
    public FloatStat gravity;

    public void Reset()
    {
        moveSpeed.Reset();
        travelDist.Reset();
        gravity.Reset();
    }
}