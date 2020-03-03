using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour
{
    IslandRandomGenerator _irg;

    public GameObject[] tilePrefabs;

    public LayerMask layer;

    List<JH_IslandComponent> islands = new List<JH_IslandComponent>();

    Transform parent;

    private void Awake()
    {
        _irg = GetComponent<IslandRandomGenerator>();
    }

    private void Start()
    {
        parent = new GameObject().transform;
        parent.name = "Islands";

        for(int y = 0; y < 3; y++)
        for(int x = 0; x < 5; x++)
        {
            GameObject temp = SpawnIsland(x*80, y*80);
            islands.Add(temp.GetComponent<JH_IslandComponent>());
        }

        int repeat = islands.Count - 1;
        for (int i = 0; i < repeat; i++)
        {
            PutBridge(islands[i], islands[i + 1]);
        }
    }

    public GameObject SpawnIsland(int posX, int posY, int wid = 0, int hei = 0, int maxWidth = 0, int maxHeight = 0)
    {
        if(wid == 0) wid = Random.Range(30, 50);
        if(hei == 0) hei = wid / 2;
        maxWidth = (maxWidth == 0) ? IslandRandomGenerator.WIDTH : maxWidth;
        maxHeight = (maxHeight == 0) ? IslandRandomGenerator.HEIGHT : maxHeight;

        JH_Island land = _irg.SpawnRandomIsland(wid,hei, maxWidth, maxHeight);

        GameObject map = new GameObject();
        map.name = "NewIsland";
        map.AddComponent<JH_IslandComponent>();
        map.GetComponent<JH_IslandComponent>().landInfo = land;
        map.transform.SetParent(parent);

        for (int y = 0;y < land.maxHeight; y++)
        {
            for(int x = 0; x < land.maxWidth;x++)
            {
                SpawnTile(x, -y, land.arr[y, x], map.transform);
            }
        }

        map.transform.position = new Vector2(posX, posY);

        return map;
    }

    public void SpawnTile(int x, int y, int tile, Transform parent)
    {
        //공기(공백)에는 생성
        if (tile < 0) return;

        GameObject temp = Instantiate(tilePrefabs[tile]);
        
        //좌표, 부모 설정
        temp.transform.position = new Vector2(x, y);
        temp.transform.SetParent(parent);

        //레이어 설정
        temp.GetComponentInChildren(typeof(SpriteRenderer)).GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Tile_Block");
        temp.layer = LayerMask.NameToLayer("Solid Obj");
    }

    void PutBridge(JH_IslandComponent land1, JH_IslandComponent land2)
    {
        Vector2 startPos = new Vector2((int)land1.transform.position.x + land1.landInfo.endPos.x, (int)land1.transform.position.y - land1.landInfo.endPos.y);
        Vector2 endPos = new Vector2((int)land2.transform.position.x + land2.landInfo.startPos.x, (int)land2.transform.position.y - land2.landInfo.startPos.y);

        int yDiff = (int)(Mathf.Abs(startPos.y - endPos.y));

        int repeat = (int)(Vector2.Distance(startPos, endPos) / 8);

        /*
        //다리로 놓을 섬 갯수 설정
        repeat = (yDiff / 4 > repeat) ? yDiff / 6 : repeat;

        Vector2 newPos;
        //갯수에 따른 간격 설정
        Vector2 interval = (endPos - startPos) / repeat;
        */

        Vector2 interval = (endPos - startPos);

        Vector2 newPos;

        //다리로 놓을 섬 갯수 설정
        for (int i = 1; true; i++)
        {
            if ((Mathf.Abs(interval.x) / 2 + Mathf.Abs(interval.y)) / i <= 5)
            {
                repeat = i;
                break;
            }
        }
        //갯수에 따른 간격 설정
        interval /= repeat;

        Debug.Log(interval.x.ToString());
        //섬 크기
        int minWidth = Mathf.Clamp((int)interval.x, 3, 5);


        GameObject land;

        for (int i = 1; i < repeat; i++)
        {
            newPos = startPos + (interval * i);


            int wid = Random.Range(minWidth, 2);
            int hei = wid / 2;

            land = SpawnIsland((int)(newPos.x - interval.x/2+ (interval.x - wid)/2), (int)newPos.y, wid, hei, wid, hei);
            land.name = "littleIsland";
            islands.Add(land.GetComponent<JH_IslandComponent>());
        }
    }
}