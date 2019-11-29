using System;
using System.Collections.Generic;
using UnityEngine;

public struct OverlapCheckData
{
    public List<Collider2D> prevOverlaps;
    public Action<Collider2D> onEnter;
    public Action<Collider2D> onExit;

    public void Clear()
    {
        prevOverlaps.Clear();
    }

    public OverlapCheckData(Action<Collider2D> onEnter = null, Action<Collider2D> onExit = null)
    {
        prevOverlaps = new List<Collider2D>();
        this.onEnter = onEnter;
        this.onExit = onExit;
    }
}

public static class OverlapCheck_Logic
{
    public static void OverlapCheck(this OverlapCheckData data, Collider2D[] overlaps)
    {
        if (overlaps.Length == 0)
        {
            data.Clear();
            return;
        }

        // Check Exit
        for (int i = data.prevOverlaps.Count - 1; i >= 0; i--)
        {
            if (Array.Exists(overlaps, col => col == data.prevOverlaps[i]))
                continue;

            data.prevOverlaps.RemoveAt(i);
            data.onExit?.Invoke(overlaps[i]);
        }

        // Check Enter
        for (int i = 0; i < overlaps.Length; i++)
        {
            if (data.prevOverlaps.Contains(overlaps[i]))
                continue;

            data.prevOverlaps.Add(overlaps[i]);
            data.onEnter?.Invoke(overlaps[i]);
        }

    }
}
