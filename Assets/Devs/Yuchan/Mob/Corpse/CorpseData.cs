using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CorpseData
{
    public int cntMin;
    public int cntMax;
    int mSpawn_Count;

    public eCorpseSpawnMode CurrentMode;
    public System.Action<GameObject> ModeEvent;

    public Corpse Corpse;
    public int GetCount => mSpawn_Count = cntMin != cntMax ? Random.Range(cntMin, cntMax + 1) : cntMin;
    public int SpawnCount => mSpawn_Count;
}