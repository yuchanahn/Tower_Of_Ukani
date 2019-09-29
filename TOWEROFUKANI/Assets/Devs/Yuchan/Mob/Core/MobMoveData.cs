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
    // 점프 가능 높이
    // 내려가기 가능 땅 레이어, 높이.


    int m_dir;
    public enum eState
    {
        Move,
        Idle
    }

    
    public int SprDir
    {
        get
        {
            return SprTr.eulerAngles.y == 180 ? -1 : 1;
        }
        set
        {
            SprTr.eulerAngles = new Vector2(0, value == -1 ? 180 : 0);
        }
    }
    public float Speed;
    public int Dir {
        get
        {
            return m_dir;
        }
        set
        {
            if (m_dir == value) return;
            m_dir = value;
            if (!(m_dir == 0)) SprTr.eulerAngles = new Vector2(0, m_dir == -1 ? 0 : 180);
        }
    }
    public Transform SprTr;
    public float JumpHeight;
    public RandRange MoveT;
    public RandRange IdleT;
    public eState State;
}