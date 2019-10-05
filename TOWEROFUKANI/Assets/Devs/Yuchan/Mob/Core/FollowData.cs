using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct FollowData
{
    public bool followAble;
    public float dis;
    public LayerMask moveAbleGroundLayers;
    public LayerMask CantMoveGround;
}
public enum eMoveAction
{
    JumpProcess = 2,
    Jump = 3,
    DownJump = 4,
    Left = -1,
    Right = 1
}


public class Follow_Logic
{

    public static eMoveAction Follow(ref FollowData data,ref MobMoveData mvdata, ref JumpData jdata, Vector2 size, int curDir, Vector2 pos)
    {
        if (Mathf.Abs(GM.PlayerPos.x - pos.x) < 0.5f && GM.PlayerPos.y > pos.y && (Mathf.Abs(GM.PlayerPos.y - pos.y) > size.y / 2))
            return eMoveAction.Jump;
        else if (Mathf.Abs(GM.PlayerPos.x - pos.x) < 0.5f && GM.PlayerPos.y < pos.y && (Mathf.Abs(GM.PlayerPos.y - pos.y) > size.y / 2))
            return eMoveAction.DownJump;

        var hit = Physics2D.RaycastAll(pos + curDir * new Vector2(size.x * 0.5f, 0), curDir * Vector2.right, 1f, data.CantMoveGround);
        Debug.DrawRay(pos + curDir * new Vector2(size.x * 0.5f, 0), curDir * Vector2.right * 1f, Color.green);

        if (hit.Length == 0)
        {
            return (Mathf.Sign(GM.PlayerPos.x - pos.x) == 1) ? eMoveAction.Right : eMoveAction.Left;
        }
        else
        {
            var Hit = MobYMoveDetect_Logic.GetVirJumpDetectGroundOrNull(pos, ref jdata, size, ref mvdata, data.CantMoveGround);
            //Debug.DrawRay(pos + new Vector2(0, size.y/2), new Vector2(curDir, 1), Color.green);
            if (!Hit) return eMoveAction.JumpProcess;
            else Debug.Log(Hit.name);   
            return curDir == -1 ? eMoveAction.Left : eMoveAction.Right;
        }
    }
}
