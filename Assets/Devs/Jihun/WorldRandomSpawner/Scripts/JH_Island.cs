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
    public int arrWidth;
    public int arrHeight;

    // 해당 청크 기준
    public Vector2Int startPos;
    public Vector2Int endPos;

    // 월드 기준
    public Vector2Int midPos;
    public Vector2Int pos;

    public int[,] arr;

    public JH_Island(int wid, int hei, int maxWidth = 0, int maxHeight = 0)
    {
        width = wid;
        height = hei;

        this.arrWidth = (maxWidth == 0) ? IslandRandomGenerator.WIDTH : maxWidth;
        this.arrHeight = (maxHeight == 0) ? IslandRandomGenerator.HEIGHT : maxHeight;

        arr = new int[this.arrHeight, this.arrWidth];
    }

    public void ShowArr()
    {
        string log = "";

        log += $"start pos : {startPos.ToString()}, end pos : {endPos.ToString()}\n";

        for (int i = 0; i < arrHeight; i++)
        {
            for (int j = 0; j < arrWidth; j++)
                log += arr[i, j].ToString();
            log += "\n";
        }
        Debug.Log(log);
    }

    public void LogThis()
    {
        Debug.Log($"pos.x : {pos.x.ToString()} pos.y : {pos.y.ToString()}");
    }
}
