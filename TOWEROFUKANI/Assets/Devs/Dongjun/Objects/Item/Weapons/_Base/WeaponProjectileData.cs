using System;
using UnityEngine;

[Serializable]
public struct WeaponProjectileData
{
    public IntStat damage;

    public FloatStat moveSpeed;
    public FloatStat gravity;

    [HideInInspector]
    public float curTravelDist;
    public FloatStat maxTravelDist;
}