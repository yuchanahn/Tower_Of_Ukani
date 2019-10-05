using System;
using UnityEngine;

[Serializable]
public struct WeaponProjectileData
{
    public int damage;

    public float moveSpeed;
    public float gravity;

    [HideInInspector]
    public float curTravelDist;
    public float maxTravelDist;
}