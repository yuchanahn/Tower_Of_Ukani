using System;
using UnityEngine;

[Serializable]
public struct WeaponProjectileData
{
    public AttackData attackData;

    [Space]
    public FloatStat moveSpeed;
    public FloatStat gravity;
    public FloatStat maxTravelDist;

    [HideInInspector]
    public float curTravelDist;
}