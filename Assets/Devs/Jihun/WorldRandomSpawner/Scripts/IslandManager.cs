using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class IslandManager : SingletonBase<IslandManager>
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
    public int arrWid => intervalX * col;
    public int arrHei => intervalY * row;

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
    [SerializeField] static float loadRate = 0.5f;

    Transform target;

    WaitForSeconds wait = new WaitForSeconds(loadRate);
    #endregion

    #region New최적화
    public JH_Chunk playerChunk;

    WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
    [SerializeField] int itemsPerFrame = 20;

    public int[,] allTiles;

    [SerializeField] List<JH_Chunk> spawnedChunks = new List<JH_Chunk>();
    [SerializeField] List<JH_Chunk> loadedChunks = new List<JH_Chunk>();

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

        int maxX = land.arrWidth;
        int maxY = land.arrHeight;

        //Debug.Log($"maxX : {maxX} maxY : {maxY} y : {y} arrheight : {arrHei}");

        maxX = Mathf.Clamp(maxX, 0, arrWid - x);
        y = Mathf.Clamp(y, 0, arrHei-1);

        //Debug.Log($"maxX : {maxX} maxY : {maxY}");


        for (int i = 0; i < maxX; i++)
        {
            for(int j = 0; j < maxY; j++)
            {
                posX = x + i;
                posY = y - j;

                //Debug.Log($"posX = {posX} posY = {posY} x = {x} y = {y} j = {j} i = {i}");

                if(land.arr[j, i] >= 0) allTiles[posY, posX] = land.arr[j, i];
            }
        }
    }


    void Init()
    {
        _irg = GetComponent<IslandRandomGenerator>();
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
        
        StartCoroutine(LoadTemp());
    }

    IEnumerator LoadTemp()
    {
        for (int i = 0; i < islands.Count; i++)
        {
            PutIslandInArr(islands[i]);
        }

        //게임 시작시 주변 바로 로드
        Vector2 pos = GM.PlayerPos;
        //로딩 범위 설정

        //플레이어 청크 좌표
        int playerX = (int)pos.x / JH_Chunk.chunkScale;
        int playerY = (int)pos.y / JH_Chunk.chunkScale;

        //청크 로딩
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                InstLoadChunk(new JH_Chunk(new Vector2Int(playerX + x, playerY + y)));
            }
        }

        //게임 진행중에만 반복할 것
        while (true)
        {
            ChunkLoading();
            

            yield return wait;
        }
    }


    void ChunkLoading()
    {
        Vector2Int pos = new Vector2Int((int)GM.PlayerPos.x, (int)GM.PlayerPos.y);


        //로딩 범위 설정

        //플레이어 청크 좌표
        int playerX = pos.x / JH_Chunk.chunkScale;
        int playerY = pos.y / JH_Chunk.chunkScale;

        playerChunk = loadedChunks.Find(temp => temp.pos == new Vector2(playerX, playerY));

        //청크 로딩
        //플레이어가 있는 청크는 한번에 로딩
        InstLoadChunk(new JH_Chunk(new Vector2Int(playerX, playerY)));

        //주변 청크들은 몇프레임 걸쳐서 로딩
        for(int x = -1; x <= 1; x++)
        {
            StartCoroutine(LoadChunk(new JH_Chunk(new Vector2Int(x + playerX, playerY + 1))));
            StartCoroutine(LoadChunk(new JH_Chunk(new Vector2Int(x + playerX, playerY - 1))));
        }
        StartCoroutine(LoadChunk(new JH_Chunk(new Vector2Int(playerX + 1, playerY))));
        StartCoroutine(LoadChunk(new JH_Chunk(new Vector2Int(playerX - 1, playerY))));
        
        //청크 언로딩
        for (int i = loadedChunks.Count - 1; i >= 0; i--)
        {
            StartCoroutine(UnLoadChunk(loadedChunks[i]));
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



    // 배열에서 해당 청크의 부분을 전부 돌면서 타일을 생성함
    IEnumerator LoadChunk(JH_Chunk chunk)
    {
        
        //미리 깔아놓는 변수들
        int x, y, spawnedInThisFrame = 0;
        GameObject obj;
        Vector2Int pos;

        //생성된 청크들 중에서 해당 청크가 있는지 확인
        JH_Chunk c = spawnedChunks.Find(temp => temp.pos == chunk.pos);

        //있으면 그 청크 로딩
        if(c != null)
        {
            //이미 켜져있으면 바로 나감
            if (c.active) yield break;
            for(int i = 0; i < c.tiles.Count; i++)
            {
                c.tiles[i].SetActive(true);
                spawnedInThisFrame++;

                // 프레임당 생성 개수 채웠으면
                if(spawnedInThisFrame > itemsPerFrame)
                {
                    yield return waitFrame;
                    spawnedInThisFrame = 0;
                }
            }
            loadedChunks.Add(c);
            c.active = true;
            //로딩 끝
            yield break;
        }

        //로딩 범위 설정
        int minX = chunk.startPos.x;
        int maxX = chunk.endPos.x;
        int minY = chunk.startPos.y;
        int maxY = chunk.endPos.y;
        //로딩 범위 설정

        //청크 없으면 새로 만듦
        for ( x = minX; x < maxX; x++)
        {
            for ( y = minY; y < maxY; y++)
            {
                //Debug.Log($"x : {x}, y : {y}, startPos : {minX}, endPos : {maxX}");
                // 공백이면 넘어감
                if (allTiles[y, x] < 0) continue;

                //해당 위치에 타일이 있는지 체크
                //pos : 배열에서의 좌표
                pos = new Vector2Int(x, y);

                //Debug.Log($"x : {x} y : {y} allTiles[y, x] : {allTiles[y, x].ToString()}");
                obj = SpawnTile(x, y, allTiles[y, x]);
                chunk.tiles.Add(obj);

                spawnedInThisFrame++;

                //해당 프레임에 로딩이 끝나면 다음 프레임으로 로딩을 넘김
                if (spawnedInThisFrame >= itemsPerFrame)
                {
                    yield return waitFrame;
                    spawnedInThisFrame = 0;
                }

            }
        }
        spawnedChunks.Add(chunk);
        loadedChunks.Add(chunk);
    }
    void InstLoadChunk(JH_Chunk chunk)
    {
        if (loadedChunks.Exists(temp => temp.pos == chunk.pos)) return;
        JH_Chunk c = spawnedChunks.Find(temp => temp.pos == chunk.pos);
        // 이미 존재하면 액티브만 켜줌
        if (c != null)
        {
            c.SetActiveAll(true);
            loadedChunks.Add(chunk);
            return;
        }
        //미리 깔아놓는 변수들
        int x, y;
        Vector2Int pos;
        GameObject obj;

        //로딩 범위 설정
        int minX = chunk.startPos.x;
        int maxX = chunk.endPos.x;
        int minY = chunk.startPos.y;
        int maxY = chunk.endPos.y;
        //로딩 범위 설정

        //존재 안하면 생성해줌
        for (x = minX; x < maxX; x++)
        {
            for (y = minY; y < maxY; y++)
            {
                //Debug.Log($"x : {x}, y : {y}");
                // 공백이면 넘어감
                if (allTiles[y, x] < 0) continue;

                //pos : 배열에서의 좌표
                pos = new Vector2Int(x, y);

                //새로 생성함
                //Debug.Log($"x : {x} y : {y} allTiles[y, x] : {allTiles[y, x].ToString()}");
                obj = SpawnTile(x, y, allTiles[y, x]);
                chunk.tiles.Add(obj);
            }
        }

        spawnedChunks.Add(chunk);
        loadedChunks.Add(chunk);
    }

    /// <summary>
    /// 로딩 되어 있는 청크들만 매개변수로 넣어준다.
    /// 그 청크안의 타일들을 순차적으로 꺼준다.
    /// </summary>
    /// <param name="chunk"></param>
    IEnumerator UnLoadChunk(JH_Chunk chunk)
    {
        if (chunk == null) yield break;
        Vector2 diff = chunk.pos - playerChunk.pos;
        // 플레이어와 근접한 청크라면 언로딩 하지 않음.
        if (Mathf.Abs(diff.x) < 2 && Mathf.Abs(diff.y) < 2) yield break;



        int i, unloaded = 0;

        for(i = 0; i < chunk.tiles.Count; i++)
        {
            chunk.tiles[i].SetActive(false);
            unloaded++;
            if(unloaded > itemsPerFrame)
            {
                yield return waitFrame;
                unloaded = 0;
            }
        }

        loadedChunks.Remove(chunk);
        chunk.active = false;
    }
#endregion

    // 초기 월드 생성하는 함수.
    // 내부 반복문에서 청크 갯수 설정하고 그 청크들 사이에 다리를 깔아줌.

    #region 월드 생성
    public void GenerateWorld()
    {
        int x, y;

        islandParent = new GameObject().transform;
        islandParent.gameObject.name = "Islands";

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
        //Debug.Log($"posX : {posX} , posY : {posY}");
        if(wid == 0) wid = Random.Range(30, 50);
        if(hei == 0) hei = wid / 2;
        maxWidth = (maxWidth == 0) ? IslandRandomGenerator.WIDTH : maxWidth;
        maxHeight = (maxHeight == 0) ? IslandRandomGenerator.HEIGHT : maxHeight;

        JH_Island land = _irg.GenerateIsland(wid,hei, maxWidth, maxHeight);
        land.pos = new Vector2Int(posX, posY);
        land.midPos = land.pos + new Vector2Int(land.arrWidth / 2, -land.arrHeight/2);

        return land;
    }

    public GameObject SpawnTile(int x, int y, int tile)
    {
        //공기(공백)에는 생성
        if (tile < 0) return null;

        GameObject temp = Instantiate(tilePrefabs[tile]);
        
        //좌표, 부모 설정
        temp.transform.position = new Vector2(x, y);
        temp.transform.SetParent(islandParent);

        //레이어 설정
        temp.GetComponentInChildren(typeof(SpriteRenderer)).GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Tile_Block");

        return temp;
    }
    #endregion


    #region Methods:Parts

    //다리 생성 메소드
    void PutBridge(JH_Island land1, JH_Island land2)
    {
        // 다리의 시작지점과 끝지점 설정.
        // 배열에서의 Y좌표 반전때문에 startPos와 endPos에 -를 곱해서 넣어야 함.
        Vector2 startPos = new Vector2(land1.pos.x + land1.endPos.x, land1.pos.y - land1.endPos.y);
        Vector2 endPos = new Vector2(land2.pos.x + land2.startPos.x, land2.pos.y - land2.startPos.y);

        Debug.Log($"startPos : {startPos}, endPos : {endPos}");


        //Debug.Log($"startpos : {startPos.ToString()}, endpos : {endPos.ToString()}");

        int repeat = 0, i, wid, hei;

        Vector2 interval = (endPos - startPos);

        Vector2 newPos = startPos;

        float interX = Mathf.Abs(interval.x);
        float interY = Mathf.Abs(interval.y);

        //다리로 놓을 섬 갯수 설정
        int re1, re2;
        for (i = 1; true; i++)
        {
            if (interX / i <= 20)
            {
                re1 = i;
                break;
            }
        }
        for (i = 1; true; i++)
        {
            if (interY / i <= 5)
            {
                re2 = i;
                break;
            }
        }
        repeat = Mathf.Max(re1, re2);

        //갯수에 따른 간격 설정
        interval /= repeat;

        Debug.Log( "repeat : " + repeat.ToString());

        //실질적으로 다리 생성하는 반복문
        for(i = 0; i < repeat; i++)
        {
            wid = Random.Range(10, 15);
            //마지막만 크기 조절 다시
            if(i == repeat-2)
            {
                if (endPos.x - newPos.x < 20) break;
            }
            if(i == repeat-1)
            {
                wid = (int)(endPos.x - newPos.x - 7);
            }
            hei = wid / 3;

            JH_Island land = _irg.GenerateBridge((int)newPos.x + 5, (int)newPos.y, wid, hei);

            islands.Add(land);

            //SpawnTile((int)newPos.x, (int)newPos.y + 70, (int)IslandRandomGenerator.Tile.GRASS);

            newPos += interval;
        }
    }

    
    #endregion
}

[System.Serializable]
public class JH_Chunk
{
    static public int chunkScale = 20;

    public List<GameObject> tiles = new List<GameObject>();

    //청크 기준 좌표
    public Vector2Int pos;

    //월드 기준 좌표
    public Vector2Int startPos;
    public Vector2Int endPos;

    public bool active = true;

    public JH_Chunk(Vector2Int chunkPos)
    {
        pos = chunkPos;
        startPos = pos * chunkScale;
        endPos = startPos + (Vector2Int.one * chunkScale);

        startPos.x = Mathf.Clamp(startPos.x, 0, IslandManager.Inst.arrWid - 1);
        endPos.x = Mathf.Clamp(endPos.x, 0, IslandManager.Inst.arrWid - 1);
        startPos.y = Mathf.Clamp(startPos.y, 0, IslandManager.Inst.arrHei - 1);
        endPos.y = Mathf.Clamp(endPos.y, 0, IslandManager.Inst.arrHei - 1);
    }
    
    public void SetActiveAll(bool value)
    {
        if (value == active) return;

        for(int i = 0; i < tiles.Count; i++)
        {
            tiles[i].SetActive(value);
        }

        active = value;
    }
}
