using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct FollowData
{
    public float dist;
    public LayerMask CantMoveGround;

    Transform m_pos;
    Vector2 m_size;

    void Init(Transform pos, Vector2 size)
    {
        m_pos = pos;
        Size = size;
    }

    public Vector2 Pos      => m_pos.position;
    public Vector2 Foot_Pos => new Vector2(Pos.x, (Pos.y - Size.y / 2));
    public Vector2 LB_Pos   => new Vector2(Pos.x - Size.x / 2, Pos.y - Size.y / 2);
    public Vector2 RB_Pos   => new Vector2(Pos.x + Size.x / 2, Pos.y - Size.y / 2);

    public Vector2 Size { get => m_size; set => m_size = value; }
}
public enum eMvAct
{
    JumpProcess = 2,
    Jump = 3,
    DownJump = 4,
    Left = -1,
    Right = 1,
    Keep = 5
}



public class Follow_Logic
{
    bool FollowCheckFor(Vector2 origin, Vector2 target, ref FollowData fData, ref JumpData jData)
    {
        var pos = fData.Pos;

        while (!pos.BoxHit(fData.Size, target, fData.CantMoveGround))
        {
            pos.y += fData.Size.y;
            if (((pos.y + fData.Size.y / 2) > jData.height)) return false;
        }
        return true;
    }

    void f() // follow 중일때.
    {
        // 내가 플레이어 보다 아래에 있다면......
        // 그리고 밑이 절벽이라면.....
        // 무조건 점프 수행......
        // 아니라면 그냥 쭉 이동.....
        // 가다가 벽을 만나면 
        // 발부터 점프 가능 높이까지 사이즈(올림한) 만큼 더해주면서 
        // 무조건 점프 수행.......인데 점프세기 조절...

        // 내가 플레이어 보다 위에있다면......
        // 절벽이면 걍감.....


    }


    public static eMvAct Follow(bool bNoWallForward, bool bCheckAble, float posY)
    {
        if(!bNoWallForward && GM.PlayerPos.y > posY)
        {
            return eMvAct.JumpProcess;
        }
        return eMvAct.Keep;
    }
}
