using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPS_Loader : MonoBehaviour
{
    public static void Load(string map_name, int obj_size, Transform[] walls, Vector2 map_size)
    {
        var GV = GridView.Inst[map_name][obj_size];

        foreach (var i in walls)
        {
            var pos = GV.GetNodeAtWorldPostiton(i.position).pos;

            for(int x = 0; x < obj_size; x++)
            {
                for(int y = 0; y < obj_size; y++)
                {
                    var virtual_pos = new Point(pos.row + y, pos.column + x);
                    if (    virtual_pos.column  >= map_size.x
                        ||  virtual_pos.row     >= map_size.y
                        ||  virtual_pos.column  < 0
                        ||  virtual_pos.row     < 0
                        ) continue;
                    GV.GetNodeAtWorldPostiton(virtual_pos).isObstacle = true;
                    GV.GetNodeAtWorldPostiton(virtual_pos).isVirtualWall = !(x == 0 && y == 0);
                }
            }
        }
    }
}
