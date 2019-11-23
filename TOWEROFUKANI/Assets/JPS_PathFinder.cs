using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPS_PathFinder : MonoBehaviour
{
    [SerializeField] GridView gridV;
    [SerializeField] Grid gridJPS => gridV.grid;
    public Vector2[] Find( Vector2 start, Vector2 stop )
    {
        List<Point> path = gridJPS.getPath( gridV.WorldToGrid(start), gridV.WorldToGrid(stop));
        var v3 = new Vector2[path.Count];
        for(int i =0; i < path.Count; i++)
        {
            v3[i] = gridV.getNodePosAsWorldPos(path[i]);
        }
        return v3;
        //return path.ToArray();
    }
}
