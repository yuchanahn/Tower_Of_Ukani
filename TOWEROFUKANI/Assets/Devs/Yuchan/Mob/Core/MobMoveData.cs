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

    public float Speed;


    [Range(0,10)] public float CoolTime;
    public float JumpAbleDist;
    public float FallAbleDist;
    public LayerMask FallAbleLayer;
    public LayerMask JumpAbleLayer;


    public RandRange MoveT;
    public RandRange IdleT;

    public float FallHeight;
    public eState State;
}


public class MobYMoveDetect_Logic
{
    public static bool UP(Vector2 pos, ref JumpData jdata, ref MobMoveData data)
    {
        var hit = Physics2D.RaycastAll(pos, new Vector2(data.Dir, jdata.height), data.JumpAbleDist, data.JumpAbleLayer);
        //Debug.DrawRay(pos, new Vector2(data.Dir, jdata.height + data.JumpAbleDist), Color.red);
        return hit.Length > 0;
    }
    public static bool Down(Vector2 pos, Vector2 size, ref MobMoveData data)
    {
        var hit = Physics2D.RaycastAll(pos - new Vector2(0, size.y * 0.5f), Vector2.down, 0.1f, data.FallAbleLayer);
        Debug.DrawRay(pos - new Vector2(0, size.y * 0.5f), Vector2.down * new Vector2(0, data.FallAbleDist), Color.red);
        if (hit.Length == 0) return false;

        hit = Physics2D.RaycastAll(pos - new Vector2(0, size.y * 0.5f), Vector2.down , data.FallAbleDist, data.FallAbleLayer);
        Debug.DrawRay(pos - new Vector2(0, size.y * 0.5f), Vector2.down * new Vector2(0, data.FallAbleDist), Color.red);
        return hit.Length > 0;
    }


    public static Collider2D GetVirJumpDetectGroundOrNull(Vector2 pos, ref JumpData jdata, Vector2 size, ref MobMoveData data, LayerMask soildGorund)
    {
        var _pos = pos + new Vector2(0, jdata.height);
        var _dist = jdata.time * data.Speed;
        var hit = Physics2D.BoxCast(_pos, size, 0, Vector2.right * data.Dir, _dist, soildGorund);
        
        return hit.collider;
    }
}