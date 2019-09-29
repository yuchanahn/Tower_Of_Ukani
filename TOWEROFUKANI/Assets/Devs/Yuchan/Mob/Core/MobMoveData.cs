using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct RandRange
{
    public float min;
    public float max;

    public float Get => UnityEngine.Random.Range(min, max);
}

[Serializable]
public struct MobMoveData
{
    public enum eState
    {
        Move,
        Idle
    }

    public float Speed;
    public int Dir;
    public float JumpHeight;
    public RandRange MoveT;
    public RandRange IdleT;
    public eState State;
}