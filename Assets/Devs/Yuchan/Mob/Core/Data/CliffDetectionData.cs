using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Dongjun.Helper;
using UnityEditor.PackageManager;

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

    static public bool CanGo(Vector2 Pos, int Dir, float Speed, Vector2 size, LayerMask wallLayer, bool show = false)
    {
        var sx = Pos - new Vector2(size.x / 2, 0);
        sx.y += size.y / 2;
        size.y *= 0.99f;
        if (show)
        {
            for (float i = 0.005f; sx.x < (Pos + new Vector2(size.x / 2, 0)).x + (Dir * Speed * Time.fixedDeltaTime); sx.x += i)
            {
                Debug.DrawRay(sx, (Vector2.down * size.y), Color.red);
            }
        }

        var obj = Physics2D.BoxCastAll(Pos, size, 0, Vector2.right * Dir, (Speed * Time.fixedDeltaTime), wallLayer);

 

        return !(obj.Length > 0);
    }

    static RaycastHit2D[] CheckForword(Vector2 Start, Vector2 Size, float Dir, float Dist, LayerMask lyr)
    {
        return Physics2D.BoxCastAll(Start, Size, 0, Vector2.right * Dir, Dist, lyr);
    }

    static Point GetGridPos(RaycastHit2D t)
    {
        return GridView.Inst[GM.CurMapName][1].GetNodeAtWorldPostiton(t.transform.position).pos;
    }

    static Vector2 GetPointToPos(Point p)
    {
        return GridView.Inst[GM.CurMapName][1].getNodePosAsWorldPos(p);
    }

    static bool IsWall(Point point)
    {
        return GridView.Inst[GM.CurMapName][1].grid.GetNode(point).isObstacle;
    }

    static public bool CanGo2(Vector2 Pos1, float JumpHeight, int Dir, float Speed, Vector2 size, LayerMask wallLayer, bool show = false)
    {
        size.y *= 0.99f;

        bool Possible = false;
        var MaxHeightY = Pos1.Add(y: JumpHeight).y;

        // 먼저 현위치에서 점프 높이까지 수직으로 검사.
        var c = Physics2D.BoxCastAll(Pos1, size, 0, Vector2.up, JumpHeight, wallLayer);

        // 위에 벽에 걸렸을 경우.
        if (c.Length > 0)
        {
            c.OrderByDescending(d => d.collider.transform.position.y);
            MaxHeightY = c[c.Length -1].transform.position.Foot(Vector2.one).y;
        }

        // 벽의 Y좌표를 점프 최대 높이로 계산한 뒤 검사를 시작한다.
        var check = CheckForword(Pos1, size, Dir, Speed * Time.fixedDeltaTime, wallLayer);
        if (check.Length == 0) return true;
        check.OrderByDescending(d => d.collider.transform.position.y);

        int NotWallStack = 0;
        
        var point = GetGridPos(check[check.Length - 1]);


        if(JumpHeight <= check.Length)
        {
            return false;
        }

        for (var i = 0; i < JumpHeight + size.y; i++, --point.row)
        {
            if (!IsWall(point))
            {
                //GameObject.Instantiate(GM.test_mark_, GetPointToPos(point), Quaternion.identity);
                Possible = Possible ? true : (size.y <= ++NotWallStack);
            }
            else
            {
                //GameObject.Instantiate(GM.test_mark_2, GetPointToPos(point), Quaternion.identity);
                NotWallStack = 0;
            }
        }
        return Possible;
    }

}