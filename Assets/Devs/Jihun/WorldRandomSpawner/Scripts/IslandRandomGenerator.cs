using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandRandomGenerator : MonoBehaviour
{
    public static int WIDTH = 60;
    public static int HEIGHT = 60;

    #region Var:SpawningRules
    public enum Tile
    {
        AIR = -1,
        GRASS,
        DIRT,
        ROCK,

    }
    int maxWidth = 50;
    int minWidth = 30;
    int maxHeight = 30;
    int minHeight = 10;

    int[] groundPer = {0, 0, 30, 50, 70, 100 };
    int[] groundSlpPer = {0, 50, 100 };
    int[] underGroundPer = { 33,66,100 };
    #endregion

    private void Start()
    {
    }
    public JH_Island SpawnRandomIsland(int wid = 0, int hei = 0, int maxWidth = 0, int maxHeight = 0)
    {
        //넓이와 높이 설정
        wid = (wid == 0) ? Random.Range(minWidth, maxWidth) : wid;
        hei = (hei == 0) ? Random.Range(minHeight, maxHeight) : hei;
        maxWidth = (maxWidth == 0) ? WIDTH : maxWidth;
        maxHeight = (maxHeight == 0) ? HEIGHT : maxHeight;

        JH_Island land = new JH_Island(wid, hei, maxWidth, maxHeight);

        InitLand(land);

        SetGround(land);

        CarveUnderground(land);

        return land;
    }

    void InitLand(JH_Island land)
    {
        for (int y = 0; y < land.maxHeight; y++)
        {
            for (int x = 0; x < land.maxWidth; x++)
            {
                land.arr[y, x] = (int)Tile.AIR;
            }
        }
    }

    void SetGround(JH_Island land)
    {
        //그리드에서 선의 시작점의 좌표를 잡는다.
        int x, y, count = 0;

        x = Random.Range(0, land.maxWidth - land.width);
        y = Random.Range(0, land.maxHeight - land.height);

        land.startPos = new Vector2Int(x, y);


        while (true)
        {
            //확률테이블 값에 따라 칸수 채우기
            int repeat = GetPercentage(groundPer);
            repeat = (repeat > land.width - count) ? (land.width - count) : repeat;

            for (int i = 0; i < repeat; i++,count++)
            {
                //Debug.Log($"x : {(x + count).ToString()}, y : {y.ToString()}, maxWidth : {land.maxWidth}, maxHeight : {land.maxHeight}");
                land.arr[y, x+count] = (int)Tile.GRASS;
                SetUnderGround(land, x+count, y);
            }

            //지상의 경사를 만듦
            int slp = (int)(GetPercentage(groundSlpPer) * (land.width/30f));

            // 다 그리면 나감
            if (count == land.width)
            {
                land.endPos = new Vector2Int(x + count - 1, y);

                break;
            }

            if (Random.Range(0,2) == 0)
            {
                if (y - slp < 0) y+= slp;
                else y-= slp;
            }
            else
            {
                if (y + slp > land.maxHeight - 1) y -= slp;
                else y+= slp;
            }

        }
    }
    void SetUnderGround(JH_Island land, int x, int y)
    {
        int repeat = GetPercentage(underGroundPer);
        repeat = (Random.Range(0, 2) == 0) ? repeat + land.height : -repeat + land.height;
        repeat = Mathf.Clamp(repeat, 1, land.maxHeight-y);

        for(int i = 1; i < repeat; i++)
        {
            land.arr[y + i, x] = (int)Tile.DIRT;
        }
    }

    //width = 30, height = 10
    //width/5 = 6;
    void CarveUnderground(JH_Island land)
    {
        int x, y, j, repeat;
        for(repeat = 0; repeat < land.width/5; repeat++)
        {
            //왼쪽 부터
            x = (int)land.startPos.x + repeat;
            //y값 구하기
            for(y = 0; y < land.maxHeight; y++)
            {
                if (land.arr[y, x] == (int)Tile.GRASS) break;
            }
            for (j = (int)(2 * (repeat + land.height / 5f)); (y + j) < land.maxHeight; j++)
            {
                land.arr[y + j, x] = (int)Tile.AIR;
            }

            //오른쪽 부터
            x = (int)land.endPos.x - repeat;
            //y값 구하기
            for (y = 0; y < land.maxHeight; y++)
            {
                if (land.arr[y, x] == (int)Tile.GRASS) break;
            }
            for (j = (int)(2 * (repeat + land.height/5f)); (y + j) < land.maxHeight; j++)
            {
                land.arr[y + j, x] = (int)Tile.AIR;
            }
        }
    }



    int GetPercentage(int[] perArr)
    {
        int ran = Random.Range(0, 100);

        for (int i = 0; i < perArr.Length; i++)
        {
            if (ran <= perArr[i])
            {
                return i;
            }
        }

        return 0;
    }
}