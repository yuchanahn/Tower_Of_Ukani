using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnData
{
    public Mob_Base mob;
    public int chance;
}


public class MobSpawner : MonoBehaviour
{
    public SpawnData[] table;

    public void Spawn(Vector2 pos)
    {
        var gv = GridView.Inst[GM.CurMapName][1];
        while(gv.GetNodeAtWorldPostiton(pos).isObstacle) pos.x += 1;
        
        table.for_each(x=>ARandom.Get(x.chance).IF(()=>Instantiate(x.mob, pos, Quaternion.identity)));
    }
}
