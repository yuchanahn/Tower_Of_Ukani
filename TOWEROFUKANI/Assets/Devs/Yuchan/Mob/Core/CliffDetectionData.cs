using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CliffDetect_Logic
{
    static public bool CanFall(float fallHeight, Transform Tr, float sizeX, LayerMask grdLayer)
    {
        int uns = sizeX == 0 ? 0 : sizeX > 0 ? 1 : -1;
        var hit = Physics2D.RaycastAll(Tr.position, new Vector2(uns, -1), Mathf.Abs(sizeX), grdLayer);
        var hit2 = Physics2D.RaycastAll(Tr.position + new Vector3(sizeX * 0.5f, 0), new Vector2(0, -1), fallHeight, grdLayer);
        Debug.DrawRay(Tr.position, new Vector2(uns, -1) * Mathf.Abs(sizeX), Color.red);
        Debug.DrawRay(Tr.position + new Vector3(sizeX * 0.5f, 0), new Vector2(0, -1) * fallHeight, Color.red);
        return (hit.Length > 0) || (hit2.Length > 0);
    }
}