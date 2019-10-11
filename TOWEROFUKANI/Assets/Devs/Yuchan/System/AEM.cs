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
}