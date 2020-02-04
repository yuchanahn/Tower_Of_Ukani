using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CorpseData
{
    public int cntMin;
    public int cntMax;

    public eCorpseSpawnMode CurrentMode;
    public System.Action<GameObject> ModeEvent;

    public Corpse Corpse;
    public int Count => cntMin != cntMax ? Random.Range(cntMin, cntMax + 1) : cntMin;
}