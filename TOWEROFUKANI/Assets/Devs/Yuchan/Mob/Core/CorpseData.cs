using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CorpseData
{
    public int mCntMin;
    public int mCntMax;

    public int mFCntMin;
    public int mFCntMax;

    public Corpse mCorpse;
    public int Count => mCntMin != mCntMax ? Random.Range(mCntMin, mCntMax + 1) : mCntMin;
}