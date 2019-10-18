using Shiroi.Pathfinding2D.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AEM
{

    public static Vector2 Foot(this Vector2 pos, Vector2 size) => new Vector2(pos.x, pos.y - size.y / 2);
    public static bool RayHit(this Vector2 pos, Vector2 target, LayerMask wallLayer)
    {
        Debug.DrawRay(pos, (target - pos).normalized * Vector2.Distance(pos, target), Color.green);
        return Physics2D.Raycast(pos, (target - pos).normalized, Vector2.Distance(pos, target), wallLayer);
    }
    public static bool RayHit(this GameObject pos, Vector2 target, LayerMask wallLayer)
    {
        Vector2 v2Pos = pos.transform.position;
        Debug.DrawRay(v2Pos, (target - v2Pos).normalized * Vector2.Distance(v2Pos, target), Color.green);
        return Physics2D.Raycast(v2Pos, (target - v2Pos).normalized, Vector2.Distance(v2Pos, target), wallLayer);
    }

    public static bool RayHit(this Vector3 pos, Vector2 target, LayerMask wallLayer)
    {
        Vector2 v2Pos = pos;
        Debug.DrawRay(pos, (target - v2Pos).normalized * Vector2.Distance(pos, target), Color.green);
        return Physics2D.Raycast(pos, (target - v2Pos).normalized, Vector2.Distance(pos, target), wallLayer);
    }

    public static bool BoxHit(this Vector3 pos, Vector2 size, Vector2 target, LayerMask wallLayer)
    {
        Vector2 v2Pos = pos;
        Debug.DrawRay(pos, (target - v2Pos).normalized * Vector2.Distance(pos, target), Color.green);
        return Physics2D.BoxCast(pos, size * 0.9f, 0, (target - v2Pos).normalized, Vector2.Distance(pos, target), wallLayer);
    }


    public static bool BoxHit(this Vector2 pos, Vector2 size, Vector2 target, LayerMask wallLayer)
    {
        Vector2 v2Pos = pos;
        Debug.DrawRay(pos, (target - v2Pos).normalized * Vector2.Distance(pos, target), Color.green);
        return Physics2D.BoxCast(pos, size * 0.9f, 0, (target - v2Pos).normalized, Vector2.Distance(pos, target), wallLayer);
    }

    public static RaycastHit2D GetBoxHit(this Vector3 pos, Vector2 size, Vector2 target, LayerMask wallLayer)
    {
        Vector2 v2Pos = pos;
        Debug.DrawRay(pos, (target - v2Pos).normalized * Vector2.Distance(pos, target), Color.green);
        return Physics2D.BoxCast(pos, size * 0.9f, 0, (target - v2Pos).normalized, Vector2.Distance(pos, target), wallLayer);
    }

    public static RaycastHit2D GetBoxHit(this Vector2 pos, Vector2 size, Vector2 target, LayerMask wallLayer)
    {
        Vector2 v2Pos = pos;
        Debug.DrawRay(pos, (target - v2Pos).normalized * Vector2.Distance(pos, target), Color.green);
        return Physics2D.BoxCast(pos, size * 0.9f, 0, (target - v2Pos).normalized, Vector2.Distance(pos, target), wallLayer);
    }


    public static RaycastHit2D GetRayHit(this Vector3 pos, Vector2 target, LayerMask wallLayer)
    {
        Vector2 v2Pos = pos;
        Debug.DrawRay(pos, (target - v2Pos).normalized * Vector2.Distance(pos, target), Color.green);
        var hitp = Physics2D.Raycast(pos, (target - v2Pos).normalized, Vector2.Distance(pos, target), wallLayer);
        return Physics2D.Raycast(pos, (target - v2Pos).normalized, Vector2.Distance(pos, target), wallLayer);
    }


    public static Vector3Int GetCellPos(this Vector3 pos, LayerMask groundLayer)
    {
        var rayhit = Physics2D.Raycast(pos, Vector3.down, 10, groundLayer);
        if (rayhit)
        {
            return (Vector3Int)PathFinder.Inst.Grid.WorldToCell(rayhit.point);
        }
        else
        {
            return Vector3Int.zero;
        }
    }

    public static Vector3 GetGorundOfBottomPos(this Vector3 pos, Vector2 size, LayerMask groundLayer)
    {
        var rayhit_mid = Physics2D.Raycast(pos + new Vector3(size.x / 2, 0), Vector3.down, 10, groundLayer);
        var rayhit_left = Physics2D.Raycast(pos, Vector3.down, 10, groundLayer);
        var rayhit_rigth = Physics2D.Raycast(pos - new Vector3(size.x / 2, 0), Vector3.down, 10, groundLayer);

        if(!rayhit_mid && !rayhit_left && !rayhit_rigth) return Vector3.zero;

        Vector2 max = rayhit_mid.point;
        max = max.y < rayhit_left.point.y ? rayhit_left.point : max;
        max = max.y < rayhit_rigth.point.y ? rayhit_rigth.point : max;

        return max;
    }
}