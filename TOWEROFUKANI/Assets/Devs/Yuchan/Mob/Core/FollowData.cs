using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FollowData
{
    public bool followAble;
    public float dis;
    public Vector2 size;
    public LayerMask moveAbleGroundLayers;
}



public class MobFollow_Logic
{
    public enum eFollowState
    {
        Jump,
        DownJump,
        Move,
        Last,
    }
     public static eFollowState Follow(Vector2 pos, FollowData data)
    {
        if (data.followAble && Mathf.Abs(GM.PlayerPos.y - pos.y) > data.size.y)
        {
            var dir = GM.PlayerPos.y > pos.y ? 1 : -1;
            var hit = Physics2D.RaycastAll(pos, new Vector2(0, dir), data.dis, data.moveAbleGroundLayers);
            Debug.DrawRay(pos, new Vector2(0, dir) * data.dis, Color.red, 0.1f);

            if (hit.Length > 0)
            {
                ATimer.Set("followEv", 1f, () => { data.followAble = true; });
                data.followAble = false;
                if (dir == 1)       return eFollowState.Jump;
                else if (dir == -1) return eFollowState.DownJump;
            }
        }
        else return eFollowState.Move;
        return eFollowState.Last;
    }
}
