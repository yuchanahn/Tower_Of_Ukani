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
    #endregion

    #region Var:최적화
    [SerializeField] float loadRate = 1f;
    [SerializeField] int loadRange = 40;

    Transform target;

    WaitForSeconds wait => new WaitForSeconds(loadRate);
    #endregion

    private void Awake()
    {
        _irg = GetComponent<IslandRandomGenerator>();
    }

    void Init()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        Init();
        GenerateWorld();

        //테스트 용
        for(int i = 0; i < islands.Count; i++)
        {
            JH_IslandComponent islandComp = SpawnIsland(islands[i]);
            islandsInField.Add(islandComp);
            islandComp.gameObject.SetActive(false);
        }
        //

        StartCoroutine(LoadTemp());
    }

    IEnumerator LoadTemp()
    {
        //게임 진행중에만 반복할 것
        while (true)
        {
            Load();
            

            yield return wait;
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
        int row = 9, col = 9, x, y;

        islandParent = new GameObject().transform;
        islandParent.name = "Islands";

        for (y = 0; y < row; y++)
            for (x = 0; x < col; x++)
            {
                islands.Add(GenerateIsland(x * 80, y * 70));
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
