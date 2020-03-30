using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JPS_Build_Manager : MonoBehaviour
{
    public void Build()
    {
        foreach (var i in GM.JPSObjSizes)
        {
            int glayer = GM.SoildGroundLayer.ToLayer();
            JPS_Loader.Load(
                name, 
                i,
                (from c in GetComponentsInChildren<Tile>()
                 where c.gameObject.layer == glayer
                 select c.transform
                ).ToArray()
            ,GM.MapSizeOf[name]);
        }
    }
}
