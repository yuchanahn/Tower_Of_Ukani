using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class IslandSpawner : MonoBehaviour
{
    #region Var:기본
    IslandRandomGenerator _irg;

    public GameObject[] tilePrefabs;

    List<JH_Island> islands = new List<JH_Island>();
    List<JH_IslandComponent> islandsInField = new List<JH_IslandComponent>();

    Transform islandParent;

    int islandIntervalX = 20;
    int islandIntervalY = 10;
    int intervalX => islandIntervalX + IslandRandomGenerator.WIDTH;
    int intervalY => islandIntervalY + IslandRandomGenerator.HEIGHT;
    int arrWid => intervalX * col;
    int arrHei => intervalY * row;

    /// <summary>
    /// x
    /// </summary>
    int col = 9;
    /// <summary>
    /// y
    /// </summary>
    int row = 9;
    #endregion

    #region Var:최적화
    [SerializeField] float loadRate = 1f;
    [SerializeField] int loadRange = 40;

    Transform target;

    WaitForSeconds wait => new WaitForSeconds(loadRate);
    #endregion


    #region New최적화
    public static int CHUNKSCALE = 20;

    public int[,] allTiles;

    List<GameObject> spawnedTiles = new List<GameObject>();
    Queue<GameObject> loadedTiles = new Queue<GameObject>();

    void InitArr()
    {
        allTiles = new int[arrHei, arrWid];

        for(int y = 0; y < arrHei; y++)
        {
            for(int x = 0; x < arrWid; x++)
            {
                allTiles[y, x] = (int)IslandRandomGenerator.Tile.AIR;
            }
        }
    }

    void PutIslandInArr(JH_Island land)
    {
        int posX, posY;
        int x = land.pos.x;
        int y = land.pos.y + intervalY - 1;

        for(int i = 0; i < land.arrWidth; i++)
        {
            for(int j = 0; j < land.arrHeight; j++)
            {
                posX = x + i;
                posY = y - j;

               // Debug.Log($"posX = {posX} posY = {posY} x = {x} y = {y} j = {j} i = {i}");

                allTiles[posY, posX] = land.arr[j, i];
            }
        }
    }


    Vector2 WorldToArr(Vector2 pos)
    {
        Vector2 newPos = Vector2.zero;

        newPos.x = pos.x;
        newPos.y = pos.y + intervalY;

        return newPos;
    }

    Vector2 ArrToWorld(Vector2 pos)
    {
        Vector2 newPos = Vector2.zero;

        newPos.x = pos.x;
        newPos.y = pos.y - intervalY;

        return newPos;
    }

    #endregion

    private void Awake()
    {
        _irg = GetComponent<IslandRandomGenerator>();
    }

    void Init()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        InitArr();
    }

    private void Start()
    {
        Init();
        GenerateWorld();

        //테스트 용
        /*
        for(int i = 0; i < islands.Count; i++)
        {
            JH_IslandComponent islandComp = SpawnIsland(islands[i]);
            islandsInField.Add(islandComp);
            islandComp.gameObject.SetActive(false);
        }
        */
        //
        for(int i = 0; i < islands.Count; i++)
        {
            PutIslandInArr(islands[i]);
        }
        
        StartCoroutine(LoadTemp());
    }

    IEnumerator LoadTemp()
    {
        //게임 진행중에만 반복할 것
        while (true)
        {
            ChunkLoading();
            

            yield return wait;
        }
    }

    /*
    IEnumerator LoadTile(GameObject prefab, int count, int frames)
    {
        int itemsPerFrame = Mathf.FloorToInt(count / frames);
        int spawned = 0;
        while (spawned < count)
        {
            for (int i = 0; i < itemsPerFrame && spawned < count; i++)
            {
                Instantiate(prefab);
                spawned++;
            }
            yield return new WaitForEndOfFrame();
        }
    }*/

    void ChunkLoading()
    {
        Vector2 pos = WorldToArr(GM.PlayerPos);
        //로딩 범위 설정

        //플레이어 청크 좌표
        int playerX = (int)pos.x / CHUNKSCALE;
        int playerY = (int)pos.y / CHUNKSCALE;

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                LoadChunk(x + playerX, y + playerY);
            }
        }
        for(int i = -1; i <= 1; i++)
        {
            UnLoadChunk(playerX + i, playerY + 2);
            UnLoadChunk(playerX - 2, playerY + i);
            UnLoadChunk(playerX + i, playerY - 2);
            UnLoadChunk(playerX + 2, playerY + i);
        }
    }

    // 배열에서 해당 청크의 부분을 전부 돌면서 타일을 생성함
    void LoadChunk(int chunkX, int chunkY)
    {
        Vector2 pos = new Vector2(chunkX * CHUNKSCALE, chunkY * CHUNKSCALE);
        GameObject obj;
        //로딩 범위 설정
        int minX = (int)pos.x;
        int maxX = (int)pos.x + CHUNKSCALE;
        int minY = (int)pos.y;
        int maxY = (int)pos.y + CHUNKSCALE;

        minX = Mathf.Clamp(minX, 0, arrWid);
        maxX = Mathf.Clamp(maxX, 0, arrWid);
        minY = Mathf.Clamp(minY, 0, arrHei);
        maxY = Mathf.Clamp(maxY, 0, arrHei);
        //로딩 범위 설정

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                // 공백이면 넘어감
                if (allTiles[y, x] < 0) continue;

                //해당 위치에 타일이 있는지 체크
                //pos : 배열에서의 좌표
                pos = new Vector2(x, y);
                obj = spawnedTiles.Find(temp => WorldToArr(temp.transform.position) == pos);

                //생성 해놓은거 킴
                if (obj)
                {
                    if (obj.activeSelf) return;
                    obj.SetActive(true);

                    loadedTiles.Enqueue(obj);
                }
                //새로 생성함
                else
                {
                    //Debug.Log($"x : {x} y : {y} allTiles[y, x] : {allTiles[y, x].ToString()}");
                    obj = Instantiate(tilePrefabs[allTiles[y, x]]);
                    spawnedTiles.Add(obj);
                    obj.transform.position = ArrToWorld(pos);

                    loadedTiles.Enqueue(obj);
                }
            }
        }
    }

    void UnLoadChunk(int chunkX, int chunkY)
    {
        Vector2 pos = new Vector2(chunkX * CHUNKSCALE, chunkY * CHUNKSCALE);
        GameObject obj;
        //로딩 범위 설정
        int minX = (int)pos.x;
        int maxX = (int)pos.x + CHUNKSCALE;
        int minY = (int)pos.y;
        int maxY = (int)pos.y + CHUNKSCALE;

        minX = Mathf.Clamp(minX, 0, arrWid);
        maxX = Mathf.Clamp(maxX, 0, arrWid);
        minY = Mathf.Clamp(minY, 0, arrHei);
        maxY = Mathf.Clamp(maxY, 0, arrHei);

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                // 공백이면 넘어감
                if (allTiles[y, x] < 0) continue;

                //해당 위치에 타일이 있는지 체크
                //pos : 배열에서의 좌표
                pos = new Vector2(x, y);
                obj = spawnedTiles.Find(temp => WorldToArr(temp.transform.position) == pos);

                //생성 해놓은거 킴
                if (obj)
                {
                    //이미 로딩이 안되어있을경우 함수 종료
                    if (!obj.activeSelf) return;

                    obj.SetActive(false);

                    loadedTiles.Enqueue(obj);
                }
                //아직 생성이 안되어있을경우 함수 종료
                else return;
            }
        }
    }
    void Load2()
    {
        Vector2 pos = WorldToArr(GM.PlayerPos);
        GameObject obj;
        //로딩 범위 설정
        int minX = (int)pos.x - (loadRange / 2);
        int maxX = (int)pos.x + (loadRange / 2);
        int minY = (int)pos.y - (loadRange / 2);
        int maxY = (int)pos.y + (loadRange / 2);

        minX = Mathf.Clamp(minX, 0, arrWid);
        maxX = Mathf.Clamp(maxX, 0, arrWid);
        minY = Mathf.Clamp(minY, 0, arrHei);
        maxY = Mathf.Clamp(maxY, 0, arrHei);
        //로딩 범위 설정

        //로딩되어있던 타일들 전부 꺼줌
        while(loadedTiles.Count > 0)
        {
            GameObject temp = loadedTiles.Dequeue();
            pos = WorldToArr(temp.transform.position);
            if(pos.x < minX || pos.x > maxX || pos.y < minY || pos.y > maxY)
                temp.SetActive(false);
        }

        for(int x = minX; x < maxX; x++)
        {
            for(int y = minY; y < maxY; y++)
            {
                // 공백이면 넘어감
                if (allTiles[y, x] < 0) continue;

                //해당 위치에 타일이 있는지 체크
                //pos : 배열에서의 좌표
                pos = new Vector2(x, y);
                obj = spawnedTiles.Find(temp => WorldToArr(temp.transform.position) == pos);
                if (obj)
                {
                    obj.SetActive(true);

                    loadedTiles.Enqueue(obj);
                }
                else
                {
                    //Debug.Log($"x : {x} y : {y} allTiles[y, x] : {allTiles[y, x].ToString()}");
                    obj = Instantiate(tilePrefabs[allTiles[y, x]]);
                    spawnedTiles.Add(obj);
                    obj.transform.position = ArrToWorld(pos);

                    loadedTiles.Enqueue(obj);
                }
            }
        }
    }

    void Load()
    {
        JH_Island land;
        JH_IslandComponent landComp;

        for (int i = 0; i < islands.Count; i++)
        {
            land = islands[i];

            landComp = islandsInField.Find(x => x.landInfo == land);

            //이미 생성 해 놓은 섬들 처리
            if (landComp)
            {
                landComp.gameObject.SetActive(Vector2.Distance(target.position, land.midPos) < loadRange);
                continue;
            }
            
            //범위 안에 있고 아직 생성되지 않은 섬들 로딩
            if (Vector2.Distance(target.position, land.midPos) < loadRange)
            {
                islandsInField.Add(SpawnIsland(land));
            }
        }
    }

    // 초기 월드 생성하는 함수.
    // 내부 반복문에서 청크 갯수 설정하고 그 청크들 사이에 다리를 깔아줌.
    public void GenerateWorld()
    {
        int x, y;

        islandParent = new GameObject().transform;
        islandParent.name = "Islands";

        for (y = 0; y < row; y++)
            for (x = 0; x < col; x++)
            {
                islands.Add(GenerateIsland(x * intervalX, y * intervalY));
            }

        //섬간 다리 생성
        for(y = 0; y < row; y++)
        {
            //층간 연결
            if (y != row-1)
                PutBridge(islands[y * (col + 1)], islands[(y + 1) * (col + 1)]);

            // 가로로 쭉쭉
            for (x = 0; x < col-1; x++)
            {
                PutBridge(islands[x + y * col], islands[x + y * col + 1]);
            }
        }
    }

    public JH_Island GenerateIsland(int posX, int posY, int wid = 0, int hei = 0, int maxWidth = 0, int maxHeight = 0)
    {
        if(wid == 0) wid = Random.Range(30, 50);
        if(hei == 0) hei = wid / 2;
        maxWidth = (maxWidth == 0) ? IslandRandomGenerator.WIDTH : maxWidth;
        maxHeight = (maxHeight == 0) ? IslandRandomGenerator.HEIGHT : maxHeight;

        JH_Island land = _irg.GenerateIsland(wid,hei, maxWidth, maxHeight);
        land.pos = new Vector2Int(posX, posY);
        land.midPos = land.pos + new Vector2Int(land.arrWidth / 2, -land.arrHeight/2);

        return land;
    }

    // 섬을 매개변수로 받아와서 그 섬의 타일을 깔아줌.
    public JH_IslandComponent SpawnIsland(JH_Island land)
    {
        GameObject map = new GameObject();
        map.name = "NewIsland";
        map.AddComponent<JH_IslandComponent>();
        map.GetComponent<JH_IslandComponent>().landInfo = land;
        map.transform.SetParent(islandParent);

        for (int y = 0; y < land.arrHeight; y++)
        {
            for (int x = 0; x < land.arrWidth; x++)
            {
                SpawnTile(x, -y, land.arr[y, x], map.transform);
            }
        }

        map.transform.position = (Vector2)land.pos;

        return map.GetComponent<JH_IslandComponent>();
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


    #region Methods:Parts

    //다리 생성 메소드
    void PutBridge(JH_Island land1, JH_Island land2)
    {
        // 다리의 시작지점과 끝지점 설정.
        // 배열에서의 Y좌표 반전때문에 startPos와 endPos에 -를 곱해서 넣어야 함.
        Vector2 startPos = new Vector2(land1.pos.x + land1.endPos.x, land1.pos.y - land1.endPos.y);
        Vector2 endPos = new Vector2(land2.pos.x + land2.startPos.x, land2.pos.y - land2.startPos.y);

        //Debug.Log($"startpos : {startPos.ToString()}, endpos : {endPos.ToString()}");

        int repeat = 0;

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

        //Debug.Log(interval.x.ToString());
        //섬 크기
        int minWidth = Mathf.Clamp((int)interval.x, 3, 5);

        //실질적으로 다리 생성하는 반복문
        for (int i = 1; i < repeat; i++)
        {
            newPos = startPos + (interval * i);

            int wid = Random.Range(minWidth, 2);
            int hei = wid / 2;

            islands.Add(GenerateIsland((int)(newPos.x - interval.x / 2 + (interval.x - wid) / 2), (int)newPos.y, wid, hei, wid, hei));
        }
    }

    
    #endregion
}
