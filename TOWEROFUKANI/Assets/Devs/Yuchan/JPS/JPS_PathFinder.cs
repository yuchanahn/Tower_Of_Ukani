using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dongjun.Helper;


public class JPS_PathFinder : MonoBehaviour
{
    [SerializeField] GridView gridV;
    [SerializeField] Grid gridJPS => gridV.grid;
    public Vector2[] Find( Vector2 start, Vector2 stop )
    {
        Vector2[] dir = {
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


        
        var v3 = new Vector2[path.Count];
        for(int i =0; i < path.Count; i++)
        {
            v3[i] = gridV.getNodePosAsWorldPos(path[i]);
        }
        return v3;
        //return path.ToArray();
    }
}
