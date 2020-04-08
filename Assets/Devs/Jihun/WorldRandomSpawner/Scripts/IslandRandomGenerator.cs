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
        WOODPLATFORM_ROD,
        WOODPLATFORM_TOP,
        WOODPLATFORM_TOPROD
    }
    int maxWidth = 50;
    int minWidth = 30;
    int maxHeight = 30;
    int minHeight = 10;

    int[] groundPer = {0, 0, 0, 0, 30, 50, 70, 100 };
    int[] groundSlpPer = {0, 50, 100 };
    int[] underGroundPer = { 33,66,100 };
    #endregion


    private void Start()
    {
    }

    #region Methods:Island
    // 섬 한개 생성하는 함수. 매개변수로 너비 높이 받아오고, 없으면 그냥 고정 청크 사이즈로 받아옴.
    public JH_Island GenerateIsland(int wid = 0, int hei = 0, int maxWidth = 0, int maxHeight = 0)
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

        for(int i = 0; i < 3; i++)
        {
            PutWoodenPlatform(land);
        }

        return land;
    }


    void InitLand(JH_Island land)
    {
        for (int y = 0; y < land.arrHeight; y++)
        {
            for (int x = 0; x < land.arrWidth; x++)
            {
                land.arr[y, x] = (int)Tile.AIR;
            }
        }
    }

    void SetGround(JH_Island land)
    {
        //그리드에서 선의 시작점의 좌표를 잡는다.
        int x, y, count = 0;

        x = Random.Range(0, land.arrWidth - land.width);
        y = Random.Range(0, land.arrHeight - land.height);

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
                if (y + slp > land.arrHeight - 1) y -= slp;
                else y+= slp;
            }

        }
    }
    void SetUnderGround(JH_Island land, int x, int y)
    {
        int repeat = GetPercentage(underGroundPer);
        repeat = (Random.Range(0, 2) == 0) ? repeat + land.height : -repeat + land.height;
        repeat = Mathf.Clamp(repeat, 1, land.arrHeight-y);

        for(int i = 1; i < repeat; i++)
        {
            land.arr[y + i, x] = (int)Tile.DIRT;
        }
    }

    void CarveUnderground(JH_Island land)
    {
        int x, y, j, repeat;
        for(repeat = 0; repeat < land.width/5; repeat++)
        {
            //왼쪽 부터
            x = (int)land.startPos.x + repeat;
            //y값 구하기
            for(y = 0; y < land.arrHeight; y++)
            {
                if (land.arr[y, x] == (int)Tile.GRASS) break;
            }
            for (j = (int)(2 * (repeat + land.height / 5f)); (y + j) < land.arrHeight; j++)
            {
                land.arr[y + j, x] = (int)Tile.AIR;
            }

            //오른쪽 부터
            x = (int)land.endPos.x - repeat;
            //y값 구하기
            for (y = 0; y < land.arrHeight; y++)
            {
                if (land.arr[y, x] == (int)Tile.GRASS) break;
            }
            for (j = (int)(2 * (repeat + land.height/5f)); (y + j) < land.arrHeight; j++)
            {
                land.arr[y + j, x] = (int)Tile.AIR;
            }
        }
    }

    void PutWoodenPlatform(JH_Island land, int x = -1, int width = 0, int height = 0)
    {
        if (land.width < 10) return;

        if (width == 0) width = Random.Range(3, 6);
        if (height == 0) height = Random.Range(3, 6);
        if (x == -1) x = Random.Range(land.startPos.x + 1, land.endPos.x - width - 1);

        Vector2Int left = Vector2Int.right * x;
        Vector2Int right = Vector2Int.right * (x + width);

        //플랫폼 다리 위치 잡아줌
        int i;
        for (i = 0; i < land.arrHeight; i++)
        {
            //Debug.Log($"x : {x}, left.x : {left.x}, i : {i}");
            if (land.arr[i, left.x] != (int)Tile.AIR)
            {
                left.y = i-1;
                break;
            }
        }
        for (i = 0; i < land.arrHeight; i++)
        {
            if (land.arr[i, right.x] != (int)Tile.AIR)
            {
                right.y = i - 1;
                break;
            }
        }
        //====================

        // 플랫폼 다리 놓아줌
        //Debug.Log($"height : {height}, width : {width}");
        int highPos = (left.y < right.y) ? left.y : right.y;

        //섬이 청크 기준 너무 높은곳에 있어 플랫폼을 놓지 못하는 상황일 때
        if (highPos < height) return;

        highPos = Mathf.Clamp(highPos - height + 1, 0, land.arrHeight-1);

        for (i = left.y; i > highPos; i--)
        {
            land.arr[i, left.x] = (int)Tile.WOODPLATFORM_ROD;
        }
        //플랫폼 다리와 판 교차점
        land.arr[i, left.x] = (int)Tile.WOODPLATFORM_TOPROD;

        for (i = right.y; i > highPos; i--)
        {
            land.arr[i, right.x] = (int)Tile.WOODPLATFORM_ROD;
        }
        //플랫폼 다리와 판 교차점
        land.arr[i, right.x] = (int)Tile.WOODPLATFORM_TOPROD;

        //판 깔아줌
        for(i = left.x-1; i <= right.x+1; i++)
        {
            if (land.arr[highPos, i] != (int)Tile.WOODPLATFORM_TOPROD) land.arr[highPos, i] = (int)Tile.WOODPLATFORM_TOP;
        }
    }
    #endregion

    #region Methods:Bridge
    //사이즈는 적당히 
    public JH_Island GenerateBridge(int posX, int posY, int wid, int hei)
    {
        JH_Island land = new JH_Island(wid, hei, wid, hei + 2);
        InitLand(land);

        land.pos = new Vector2Int(posX, posY);
        land.midPos = land.pos + new Vector2Int(land.arrWidth / 2, -land.arrHeight / 2);

        Vector2Int startPos = Vector2Int.up;
        land.startPos = startPos;
        land.endPos = startPos + Vector2Int.right * wid;
        int diffStart = Random.Range(0, wid/3*2);
        if (diffStart == 1) diffStart = 0;
        int diffEnd = Random.Range(diffStart, wid);
        if (diffEnd == wid-1) diffEnd = wid;
        int upDown = Random.Range(0, 2) == 0 ? 1 : -1;
        int x, y;

        // 표면 깔기
        for(x = 0; x < diffStart; x++)
        {
            land.arr[startPos.y, x] = (int)Tile.GRASS;
        }
        startPos.y += upDown;
        for(x = diffStart; x < diffEnd; x++)
        {
            land.arr[startPos.y, x] = (int)Tile.GRASS;
        }
        startPos.y -= upDown;
        for (x = diffEnd; x < wid; x++)
        {
            land.arr[startPos.y, x] = (int)Tile.GRASS;
        }
        // 표면 깔기
        if (upDown == -1) upDown = 0;
        //지하 채우기
        int left = 0, right = 0;
        for(y = 0; y < hei; y++)
        {
            left += Random.Range(y, 2);
            right += Random.Range(y, 2);
            for (x = left; x < wid - right; x++)
            {
                if(land.arr[y + 1 + upDown, x] < 0)
                    land.arr[y + 1 + upDown, x] = (int)Tile.DIRT;
            }
        }


        return land;
    }
    #endregion


    // 확률 테이블 관련
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