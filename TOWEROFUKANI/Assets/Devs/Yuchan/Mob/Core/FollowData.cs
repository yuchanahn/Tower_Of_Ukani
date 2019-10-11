using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct FollowData
{
    public bool followAble;

    public float dist;

    public LayerMask CantMoveGround;
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
    bool FollowCheckFor(Vector2 origin, Vector2 target)
    {
        return false;
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
