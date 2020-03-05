using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JH_Island
{
    public Transform left;
    public Transform right;

    public int width;
    public int height;
    public int maxWidth;
    public int maxHeight;

    public Vector2Int startPos;
    public Vector2Int endPos;

    public Vector2Int midPos;
    public Vector2Int pos;

    public int[,] arr;

    public JH_Island(int wid, int hei, int maxWidth = 0, int maxHeight = 0)
    {
        width = wid;
        height = hei;

        this.maxWidth = (maxWidth == 0) ? IslandRandomGenerator.WIDTH : maxWidth;
        this.maxHeight = (maxHeight == 0) ? IslandRandomGenerator.HEIGHT : maxHeight;

        arr = new int[this.maxHeight, this.maxWidth];
    }

    public void ShowArr()
    {
        string log = "";

        log += $"start pos : {startPos.ToString()}, end pos : {endPos.ToString()}\n";

        for (int i = 0; i < maxHeight; i++)
        {
            for (int j = 0; j < maxWidth; j++)
                log += arr[i, j].ToString();
            log += "\n";
        }
        Debug.Log(log);
    }
}
