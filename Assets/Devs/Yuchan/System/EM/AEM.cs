﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AEM
{
    public static float AddDT(ref this float val)
    {
        return val += Time.deltaTime;
    }


    public static List<T> Filter<T>(this List<T> l, Func<T, bool> f)
    {
        List<T> rl = new List<T>();
        foreach(var i in l)
        {
            if (f(i)) rl.Add(i);
        }
        return rl;
    }

    public static T[] Filter<T>(this T[] l, Func<T, bool> f)
    {
        List<T> rl = new List<T>();
        foreach (var i in l)
        {
            if (f(i)) rl.Add(i);
        }
        return rl.ToArray();
    }

    public static T2[] Map<T, T2>(this T[] l, Func<T, T2> f)
    {
        T2[] r = new T2[l.Length];
        for (int i = 0; i < l.Length; i++) r[i] = f(l[i]);
        return r;
    }


    public static Vector2 Foot(this Vector2 pos, Vector2 size) => new Vector2(pos.x, pos.y - size.y / 2);
    public static Vector2 Foot(this Vector3 pos, Vector2 size) => new Vector2(pos.x, pos.y - (size.y / 2));
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



    public static Vector3 GetGorundOfBottomPos(this Vector3 pos, Vector2 size, LayerMask groundLayer)
    {
        var rayhit_mid = Physics2D.Raycast(pos + new Vector3(size.x / 2, 0), Vector3.down, 10, groundLayer);
        var rayhit_left = Physics2D.Raycast(pos, Vector3.down, 10, groundLayer);
        var rayhit_rigth = Physics2D.Raycast(pos - new Vector3(size.x / 2, 0), Vector3.down, 10, groundLayer);

        if(!rayhit_mid && !rayhit_left && !rayhit_rigth) return Vector3.zero;

        Vector2 max = rayhit_mid.point;
        max = max.y < rayhit_left.point.y ? rayhit_left.point : max;
        max = max.y < rayhit_rigth.point.y ? rayhit_rigth.point : max;

        max.x = pos.x;
        return max;
    }
}