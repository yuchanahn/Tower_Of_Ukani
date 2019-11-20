using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CliffDetect_Logic
{
    static public bool IsCliff(Vector2 size, Vector2 footPos, LayerMask groundLayer)
    {
        return !footPos.BoxHit(size, footPos, groundLayer);
    }


    static public bool CanFall(float fallHeight, Transform Tr, float sizeX, LayerMask grdLayer)
    {
        int uns = sizeX == 0 ? 0 : sizeX > 0 ? 1 : -1;
        var hit = Physics2D.RaycastAll(Tr.position, new Vector2(uns, -1), Mathf.Abs(sizeX), grdLayer);
        var hit2 = Physics2D.RaycastAll(Tr.position + new Vector3(sizeX * 0.5f, 0), new Vector2(0, -1), fallHeight, grdLayer);
        //Debug.DrawRay(Tr.position, new Vector2(uns, -1) * Mathf.Abs(sizeX), Color.red);
        //Debug.DrawRay(Tr.position + new Vector3(sizeX * 0.5f, 0), new Vector2(0, -1) * fallHeight, Color.red);
        return (hit.Length > 0) || (hit2.Length > 0);
    }


    static public bool CanFall2(float fallHeight, Vector2 Pos, int Dir, float Speed, Vector2 size, LayerMask grdLayer)
    {
        var TPos = Pos += ((Vector2.right * Dir) * Speed * Time.fixedDeltaTime);
        TPos.x += size.x * Dir;
        TPos.y -= size.y/2;

        var sx = TPos - new Vector2(size.x / 2, 0);

        //for (float i = 0.005f; sx.x < (TPos + new Vector2(size.x / 2, 0)).x; sx.x += i)
        {
            //Debug.DrawRay(sx, (Vector2.down * fallHeight), Color.green);
        }
        return Physics2D.BoxCastAll(TPos, size, 0, Vector2.down, fallHeight, grdLayer).Length > 0;
    }

    static public bool CanGo(Vector2 Pos, int Dir, float Speed, Vector2 size, LayerMask wallLayer)
    {
        var sx = Pos - new Vector2(size.x / 2, 0);
        sx.y += size.y / 2;
        size.y *= 0.99f;
        //for (float i = 0.005f; sx.x < (Pos + new Vector2(size.x / 2, 0)).x + (Dir * Speed * Time.fixedDeltaTime); sx.x += i)
        {
            //Debug.DrawRay(sx, (Vector2.down * size.y), Color.red);
        }

        var obj = Physics2D.BoxCastAll(Pos, size, 0, Vector2.right * Dir, (Speed * Time.fixedDeltaTime), wallLayer);

 

        return !(obj.Length > 0);
    }
}