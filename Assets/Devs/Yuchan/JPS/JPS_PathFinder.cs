using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dongjun.Helper;


public class JPS_PathFinder : MonoBehaviour
{
    [SerializeField] GridView gridV;
    [SerializeField] GameObject pr;
    [SerializeField] bool ShowingPath;
    [SerializeField] Grid gridJPS => gridV.grid;
    [SerializeField] int mSize;

    static Dictionary<(string MapName, int Size), JPS_PathFinder> mGet = new Dictionary<(string, int), JPS_PathFinder>();
    public static JPS_PathFinder _1x1 => mGet[(GM.CurMapName, 1)];
    public static JPS_PathFinder _2x2 => mGet[(GM.CurMapName, 2)];

    private void Awake()
    {
        mGet[(gridV.MapName, mSize)] = this;
    }

    static List<GameObject> pool = new List<GameObject>();
    public Vector2[] Find(Vector2 start, Vector2 stop)
    {
        Vector2[] dir = 
        {
            new Vector2( 0, 0),
            new Vector2( 0,-1),
            new Vector2( 1, 0),
            new Vector2( 1,-1),
            new Vector2( 1, 1),
            new Vector2(-1, 0),
            new Vector2(-1,-1),
            new Vector2(-1, 1)
        };  


        if (gridV.GetNodeAtWorldPostiton(stop).isVirtualWall)
        {
            foreach (var d in dir)
            {
                for (int i = 0; i < gridV.Target_Object_Size; i++)
                {
                    for (int j = 0; j < gridV.Target_Object_Size; j++)
                    {
                        if (gridV.GetNodeAtWorldPostiton(stop.Add(d.x + i, d.y -j)).isObstacle) goto END;
                    }
                }

                stop = stop.Add(d.x, d.y);
                break;
            END: 
                continue;
            }
        }
        List<Point> path = gridJPS.getPath( gridV.WorldToGrid(start), gridV.WorldToGrid(stop));
        if (path.Count > 0)
        {
            //if (path[0].column == gridV.WorldToGrid(start).column && path[0].row == gridV.WorldToGrid(start).row)
            //{
            //    path.Remove(path[0]);
            //}
        }

        var v3 = new Vector2[path.Count];
        for(int i =0; i < path.Count; i++)
        {
            v3[i] = gridV.getNodePosAsWorldPos(path[i]);
        }

        if (ShowingPath)
        {
            var arr = pool.ToArray();
            pool.Clear();
            foreach (var i in arr)
            {
                Destroy(i);
            }
            foreach (var i in v3)
            {
                pool.Add(Instantiate(pr, i, Quaternion.identity));
            }
        }
        return v3;
    }
}
