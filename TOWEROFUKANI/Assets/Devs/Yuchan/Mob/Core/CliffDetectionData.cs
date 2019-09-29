using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CliffDetectionData
{
    public float FallHeight;
}

public class CliffDetect_Logic
{
    static public bool CanFall(CliffDetectionData data, Transform Tr, float sizeX, LayerMask grdLayer)
    {
        var hit = Physics2D.RaycastAll(Tr.position + new Vector3(sizeX * 0.5f, 0), Vector2.down, data.FallHeight, grdLayer);
        Debug.DrawRay(Tr.position + new Vector3(sizeX * 0.5f, 0), Vector2.down * data.FallHeight, Color.red);
        return hit.Length > 0;
    }
}